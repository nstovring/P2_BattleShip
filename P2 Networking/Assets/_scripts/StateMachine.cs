using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {

	public static int State = 2; //Should be zero at start of a game
	//State 0 is ShipPlacing State
	//State 1 is Mini-Game State
	//State 2 is Attacking State
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
