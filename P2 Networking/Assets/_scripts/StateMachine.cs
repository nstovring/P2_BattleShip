using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour {

	public bool shipPlacing = true;
	public bool miniGame;
	public bool attacking;
	public int teamTurn = 0;
	public float countdownTimer = 10f;
	public Text countDownTimerTextBox;
	public static int TeamTurn;
	//Is only two for testing
	public int readyPlayerMin = 4;
	public static int State = 0; //Should be zero at start of a game
	//State 0 is ShipPlacing State
	//State 1 is Mini-Game State
	//State 2 is Attacking State
	public NetworkView nView;
	// Use this for initialization
	void Start () {
		//Get this objects NetworkView
		nView = GetComponent<NetworkView>();
	}

	public int GetState(){
		return State;
	}
	
	// Update is called once per frame
	void Update () {
		//TeamTurn = teamTurn;
		TeamTurn = TeamTurn >2 ? 2: TeamTurn;

		if(shipPlacing){
			nView.RPC("ChangeState", RPCMode.AllBuffered,0);
			shipPlacing = false;
		}
		if(miniGame){
			nView.RPC("ChangeState", RPCMode.AllBuffered,1);
			miniGame = false;
		}
		if(attacking){
			nView.RPC("ChangeState", RPCMode.AllBuffered,2);
			attacking = false;
		}
	}
	int readyPlayers = 0; 
	[RPC]
	void CheckForReady(int playerReady){
		Debug.Log ("Some is Ready:" + readyPlayers);
		readyPlayers+= playerReady;
		if(readyPlayers >= readyPlayerMin){
			StartCoroutine("CountDownToNextPhase");
		//nView.RPC("ChangeState", RPCMode.AllBuffered,1);
		}else{
			StopCoroutine("CountDownToNextPhase");
		}
	}

	IEnumerator CountDownToNextPhase(){
		SetTeamTurn(0);
		countDownTimerTextBox.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 285);
		//countdownTimer -= Time.deltaTime * 1;
		//countDownTimerTextBox.text = "Calibrate your ships in: " + Mathf.RoundToInt(countdownTimer);
		while(countdownTimer > 0f){
			countdownTimer -= Time.deltaTime * 1;
			countDownTimerTextBox.text = "Calibrate your ships in: " + Mathf.RoundToInt(countdownTimer);
			yield return null;
		}
		//if(countdownTimer <= 0){
		countDownTimerTextBox.text = "GO!";
		countDownTimerTextBox.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 800);
		countdownTimer = 0f;
		readyPlayers = 0;
		nView.RPC("ChangeState",RPCMode.AllBuffered,1);
		StopCoroutine("CountDownToNextPhase");
		yield return null;
		//}
	}

	//Sets the state variable
	[RPC]
	public void ChangeState(int state){
		State = state;
	}

	[RPC]
	public void SetTeamTurn(int turn){
		TeamTurn = turn;
	}
	public int GetTeamTurn(){
		return TeamTurn;
	}

	//Reload scene method for debugging
	public void ReloadScene(){
		if(Network.isClient){
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}
		if(Network.isServer){
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}
		Application.LoadLevel(Application.loadedLevel);
	}
}
