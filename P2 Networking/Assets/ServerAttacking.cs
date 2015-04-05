using UnityEngine;
using System.Collections;

public class ServerAttacking : MonoBehaviour {

	public GameObject targetMarker;
	public GameObject GhostTargetMarker;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer){
			SelectTarget();
		}
	}

	void SelectTarget(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit, 100)){
			if(hit.transform.tag == "GridSquare"){

				DisplayGhostMarker(hit);
				//If left mousebutton pressed
				if(Input.GetMouseButtonDown(0)){
					//Instantiate some sort of attacking gameobject with a collider
					GetComponent<NetworkView>().RPC("DeployTargetMarker",RPCMode.AllBuffered, hit.transform.position);

					//if a ship is hit the ship itself will call an RPC method to notify of a hit or miss 
				}
			}
		}
	}
	void DisplayGhostMarker(RaycastHit hit){
			GhostTargetMarker.transform.position = hit.transform.position;
	}
	[RPC]
	void DeployTargetMarker(Vector3 hit){
		Debug.Log("Target Placed");
		if(Network.isServer){
		Network.Instantiate(targetMarker,hit,Quaternion.identity,0);
		}
	}

}
