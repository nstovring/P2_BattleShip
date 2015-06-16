using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerAttacking : MonoBehaviour {

	public GameObject targetMarker;
	public GameObject GhostTargetMarker;
	public StateMachine stateMachine;
	public Text countDownTimerTextBox;
	public GameObject[] targetMarkers = new GameObject[2];
	public GameObject[] ghostTargetMarkers = new GameObject[5];
	public LayerMask mylayerMask;
	public int shotAmount = 5;
	//private int gridLayer = 1<< 8;
	int currentTargetMarker = 0;
	//bool nextTurn = false;
	public float nextTurnTimer = 3f;
	public int turnsPassed = 0;
	public int maxTurns = 5;
	public float turnCountdownTimer = 3f;
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
		if(turnCountdownTimer <= 0){
			//StopCoroutine("TurnCoolDown");
		}
	}
	Transform movedMarker = null;
	void MoveMarkers(){

	}

	void SelectTarget(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit, 100,mylayerMask)){
			if(hit.transform.tag == "GridSquare"){
				if(movedMarker != null){
					movedMarker.parent.position = hit.transform.position;
					movedMarker = null;
				}else if(Input.GetMouseButtonDown(0) && currentTargetMarker < shotAmount){
					DisplayGhostMarker(hit, currentTargetMarker);
					currentTargetMarker ++;
				}
			}
			if(hit.transform.tag == "GhostTargetMarker"  && Input.GetMouseButtonDown(0)){
				movedMarker = hit.transform;
			}
		}
	}
	
	[RPC]
	void DisplayGhostMarker(RaycastHit hit, int currentMarker){
			ghostTargetMarkers[currentMarker].transform.position = hit.transform.position;
	}

	[RPC]
	void SetShots(int shots){
		shotAmount = shots;
	}


	//Change the amount of shots depending on the current turn
	[RPC]
	void CheckTurn(){
		if(turnsPassed == 0 || turnsPassed == 2){
			shotAmount = 6;
		}else if(turnsPassed == 1 || turnsPassed == 3){
			shotAmount = 4;
		}
	}

	public void FireSalvo(){
		foreach(GameObject ghostTargetMarker in ghostTargetMarkers){
			nView.RPC("DeployTargetMarker",RPCMode.AllBuffered, ghostTargetMarker.transform.position);
			ghostTargetMarker.transform.position = new Vector3(50,0,50);
		}
		currentTargetMarker = 0;
		if(stateMachine.GetTeamTurn() == 1 && turnsPassed < maxTurns){
			StartCoroutine("TurnCoolDown", 2);
		}else if(stateMachine.GetTeamTurn() == 2 && turnsPassed < maxTurns){
			StartCoroutine("TurnCoolDown", 1);
	
		}
		if(turnsPassed >= maxTurns){
			turnsPassed = 0;
			StartCoroutine("CountDownToNextPhase");
		}else{
			StopCoroutine("CountDownToNextPhase");
		}
	}

	IEnumerator TurnCoolDown(int num){
		//Wait for 2 seconds before continuing
		yield return new WaitForSeconds(2);
		stateMachine.SetTeamTurn(num);
		currentTargetMarker = 0;
		//turnsPassed++;
		nView.RPC("IncrementTurnPassed",RPCMode.AllBuffered);
		turnCountdownTimer = 3f;
		nView.RPC("CheckTurn",RPCMode.AllBuffered);
		StopCoroutine("TurnCoolDown");
	}

	[RPC]
	void IncrementTurnPassed(){
		turnsPassed++;
	}

	IEnumerator CountDownToNextPhase(){
		stateMachine.SetTeamTurn(0);
		countDownTimerTextBox.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 285);
		while(countdownTimer > 0f){
			countdownTimer -= Time.deltaTime * 1;
			countDownTimerTextBox.text = "Calibrate your ships in: " + Mathf.RoundToInt(countdownTimer);
			yield return null;
		}
		countDownTimerTextBox.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 800);

		nView.RPC("ChangeState",RPCMode.AllBuffered,1);
		countdownTimer = 11f;
		StopCoroutine("CountDownToNextPhase");

		yield return null;
	}


	[RPC]
	void DeployTargetMarker(Vector3 hit){
		Debug.Log("Target Placed");
		if(Network.isServer){
		Network.Instantiate(targetMarker,hit,Quaternion.identity,0);
		}
	}

}
