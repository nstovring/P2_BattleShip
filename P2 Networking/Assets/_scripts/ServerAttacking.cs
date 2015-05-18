using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerAttacking : MonoBehaviour {

	public GameObject targetMarker;
	public GameObject GhostTargetMarker;
	public StateMachine stateMachine;
	public Text countDownTimerTextBox;
	public GameObject[] targetMarkers = new GameObject[2];
	public GameObject[] ghostTargetMarkers = new GameObject[2];
	public LayerMask mylayerMask;
	//private int gridLayer = 1<< 8;
	int currentTargetMarker = 0;
	//bool nextTurn = false;
	public float nextTurnTimer = 3f;
	public int turns = 0;
	public int maxTurns = 5;
	public float countdownTimer = 5f;
	NetworkView nView;
	//StateMachine stateMachine;

	//int targetMarkers = 2;
	void Start(){
		nView = GetComponent<NetworkView>();
		stateMachine = GetComponent<StateMachine>();
	}
	// Update is called once per frame
	void Update () {
		if(Network.isServer && stateMachine.GetState() == 2){
			SelectTarget();
		}
	}

	void SelectTarget(){
		//nView = GetComponent<NetworkView>();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit, 100,mylayerMask)){
			//if()
			if(hit.transform.tag == "GridSquare"){
				//If left mousebutton pressed
				if(Input.GetMouseButtonDown(0) && currentTargetMarker < 2){
					//Instantiate some sort of attacking gameobject with a collider
					DisplayGhostMarker(hit, currentTargetMarker);
					currentTargetMarker ++;
				}
			}
		}
	}
	[RPC]
	void DisplayGhostMarker(RaycastHit hit, int currentMarker){
			ghostTargetMarkers[currentMarker].transform.position = hit.transform.position;
	}

	public void FireSalvo(){
		foreach(GameObject ghostTargetMarker in ghostTargetMarkers){
			nView.RPC("DeployTargetMarker",RPCMode.AllBuffered, ghostTargetMarker.transform.position);
		}
		currentTargetMarker = 0;
		//nextTurn = true;
		if(stateMachine.GetTeamTurn() == 1 && turns < maxTurns){
		stateMachine.SetTeamTurn(2);
			currentTargetMarker = 0;
			turns++;
			stateMachine.GetComponent<NetworkView> ().RPC ("winCheck",RPCMode.AllBuffered);
		}else if(stateMachine.GetTeamTurn() == 2 && turns < maxTurns){
		stateMachine.SetTeamTurn(1);
			currentTargetMarker = 0;
			turns++;
			stateMachine.GetComponent<NetworkView> ().RPC ("winCheck",RPCMode.AllBuffered);
		}
		if(turns >= maxTurns){
			turns = 0;
			StartCoroutine("CountDownToNextPhase");
			/*stateMachine.SetTeamTurn(0);
			countdownTimer -= Time.deltaTime * 1;
			countDownTimerTextBox.text = "Calibrate your ships in: " + Mathf.RoundToInt(countdownTimer);
			if(countdownTimer <= 0){
			countDownTimerTextBox.text = "GO!";
			nView.RPC("ChangeState",RPCMode.AllBuffered,1);
			turns = 0;*/
			//}
		}
	}
	
	IEnumerator CountDownToNextPhase(){
		stateMachine.SetTeamTurn(0);
		//countdownTimer -= Time.deltaTime * 1;
		countDownTimerTextBox.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 285);
		while(countdownTimer > 0f){
			countdownTimer -= Time.deltaTime * 1;
			countDownTimerTextBox.text = "Calibrate your ships in: " + Mathf.RoundToInt(countdownTimer);
			yield return null;
		}
		//if(countdownTimer <= 0){
		countDownTimerTextBox.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 800);

		countDownTimerTextBox.text = "GO!";
		nView.RPC("ChangeState",RPCMode.AllBuffered,1);
		countdownTimer = 0f;
		//turns = 0;
		StopCoroutine("CountDownToNextPhase");

		yield return null;
		//}
	}


	[RPC]
	void DeployTargetMarker(Vector3 hit){
		Debug.Log("Target Placed");
		if(Network.isServer){
		Network.Instantiate(targetMarker,hit,Quaternion.identity,0);
		}
	}

}
