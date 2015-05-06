using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {

	public bool shipPlacing;
	public bool miniGame;
	public bool attacking;
	public static int State = 0; //Should be zero at start of a game
	//State 0 is ShipPlacing State
	//State 1 is Mini-Game State
	//State 2 is Attacking State
	NetworkView nView;
	// Use this for initialization
	void Start () {
		nView = GetComponent<NetworkView>();
	}
	
	// Update is called once per frame
	void Update () {
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
		//shipPlacing = miniGame ? false : attacking ? false: shipPlacing;
		//miniGame = shipPlacing ? false : attacking ? false: miniGame; 
		//attacking = miniGame ? false : shipPlacing ? false: attacking; 
		//Debug.Log("State is: " + State);
		//State = shipPlacing ? 0 : miniGame ? 1: attacking ? 2 : State;
		//State = shipPlacing ? 0 : State;
		//State = miniGame ? 1 : State;
		//State = attacking ? 2 : State;
	}

	[RPC]
	public void ChangeState(int state){
		State = state;
	}

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
