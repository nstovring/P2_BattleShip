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
	public Text winText;
	public static int TeamTurn;
	//Is only two for testing
	public int readyPlayerMin = 4;
	public static int State = 0; //Should be zero at start of a game
	//State 0 is ShipPlacing State
	//State 1 is Mini-Game State
	//State 2 is Attacking State
	public NetworkView nView;
	
	GameObject[] team1Ships = new GameObject[10];
	GameObject[] team2Ships = new GameObject[10];
	int team1Ship = 0;
	int team2Ship = 0;
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

	[RPC]
	public void addShip(int team, GameObject ship){
		Debug.Log ("add ship called");
		if (Network.isServer) {
			if( team == 0){
				team1Ships[team1Ship] = ship;
				team1Ship +=1;
				Debug.Log("ship added to team 1");
			}
			else if(team == 1){
				team2Ships[team2Ship] = ship;
				team2Ship +=1;
				Debug.Log("ship added to team 2");
			}
		}
	}
	[RPC]
	public void winCheck(){
		if (Network.isServer) {
			Debug.Log("checking for win");
			int team1health = 0;
			int team2health = 0;
			foreach(GameObject t1 in team1Ships){
				if(t1 != null){
				team1health+=t1.GetComponent<shipScript>().health;
				}
			}
			foreach(GameObject t2 in team1Ships){
				if(t2 != null){
				team2health+=t2.GetComponent<shipScript>().health;
				}
			}
			if(team1health == 0){
				nView.RPC("winSet",RPCMode.AllBuffered,1,2);
				Debug.Log("team 1 won");
			}
			else if(team2health == 0){
				nView.RPC("winSet",RPCMode.AllBuffered,3,4);
				Debug.Log("team 2 won");
			}
		}
	}
	[RPC]
	public void winSet(int p1, int p2){
		if (Network.isClient) {
			int thisPlayer = int.Parse(Network.player.ToString());
			if(thisPlayer == p1 || thisPlayer == p2){
				winText.GetComponent<Text>().text = "You Win!";
			}
			else{
				winText.GetComponent<Text>().text = "You Lose!";
			}
		}
		if (Network.isServer) {
			winText.GetComponent<Text>().text = "GAME OVER";
		}
	}
}
