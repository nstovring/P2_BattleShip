using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
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
