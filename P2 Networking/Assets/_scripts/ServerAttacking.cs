using UnityEngine;
using System.Collections;

public class ServerAttacking : StateMachine {

	public GameObject targetMarker;
	public GameObject GhostTargetMarker;
	public StateMachine stateMachine;
	public GameObject[] targetMarkers = new GameObject[2];
	public GameObject[] ghostTargetMarkers = new GameObject[2];
	private int gridLayer = 1<< 8;
	int currentTargetMarker = 0;
	bool nextTurn = false;
	public float nextTurnTimer = 3f;
	//int targetMarkers = 2;

	// Update is called once per frame
	void Update () {
		if(Network.isServer){
			SelectTarget();
		}
	}

	void SelectTarget(){
		nView = GetComponent<NetworkView>();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit, 100,gridLayer)){
			if(hit.transform.tag == "GridSquare"){
				//if()

				//If left mousebutton pressed
				if(Input.GetMouseButtonDown(0) && currentTargetMarker < 2){
					//Instantiate some sort of attacking gameobject with a collider
					DisplayGhostMarker(hit, currentTargetMarker);
					currentTargetMarker ++;

					//if a ship is hit the ship itself will do a RPC to a method to notify of a hit or miss 
				}
			}
		}
	}
	void DisplayGhostMarker(RaycastHit hit, int currentMarker){
			ghostTargetMarkers[currentMarker].transform.position = hit.transform.position;
	}

	public void FireSalvo(){
		foreach(GameObject ghostTargetMarker in ghostTargetMarkers){
			nView.RPC("DeployTargetMarker",RPCMode.AllBuffered, ghostTargetMarker.transform.position);
		}
		currentTargetMarker = 0;
		nextTurn = true;
		if(stateMachine.GetTeamTurn() == 1){
		stateMachine.SetTeamTurn(2);
		}else if(stateMachine.GetTeamTurn() == 2){
		stateMachine.SetTeamTurn(1);
		}
	}


	[RPC]
	void DeployTargetMarker(Vector3 hit){
		Debug.Log("Target Placed");
		if(Network.isServer){
		Network.Instantiate(targetMarker,hit,Quaternion.identity,0);
		}
	}

}
