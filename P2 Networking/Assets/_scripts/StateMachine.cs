using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {

	public bool shipPlacing = true;
	public bool miniGame;
	public bool attacking;
	public int teamTurn = 0;
	public static int TeamTurn;
	//Is only two for testing
	public int readyPlayerMin = 2;
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
		nView.RPC("ChangeState", RPCMode.AllBuffered,1);
		}
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
	private void ReloadScene(){
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
