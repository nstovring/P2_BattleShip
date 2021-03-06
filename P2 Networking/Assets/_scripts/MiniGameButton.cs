﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniGameButton : MiniGameManager {


	public bool isActive = false;
	public string name;
	MiniGameManager miniGameManager;
	Text thisName;
	//MiniGameManager NetworkView
	NetworkView mNView;

	int thisPlayer;
	// Use this for initialization
	void Start () {
		//nView = new NetworkView();
		//nView.viewID = 366;
		nView = GetComponent<NetworkView>();
		miniGameManager = transform.parent.GetComponent<MiniGameManager>();
		mNView = transform.parent.GetComponent<NetworkView>();
		thisPlayer = int.Parse(Network.player.ToString());
	}

	bool setTask = false;
	// Update is called once per frame
	void Update () {
		if(isActive && !setTask ){
			setTask = true;
		}

		GetComponentInChildren<Text>().text = name;
		
	}
	//If the button is active and the clicker is a client call the RPCCompleted Task method on the server
	public void CompletedTask(){
		if(isActive && Network.isClient){
			nView.RPC("RPCCompletedTask", RPCMode.Server, int.Parse(Network.player.ToString()));
		}
		//if button not active: teamScore -1 

	}
	[RPC]
	void SetActive(){
		isActive = true;
	}

	//The server will call the Deactivate method serverSide (this is probably redundant?)
	[RPC]
	void RPCCompletedTask(int sender){
		int thisPlayer = int.Parse(Network.player.ToString());
		Debug.Log("I am " + thisPlayer + " the sender is " + sender);
		nView.RPC("Deactivate", RPCMode.Server);
	}
	//The deactivate method will call the InquireNewTask method on the MiniGameManagers networkView serverside
	[RPC]
	void Deactivate(NetworkMessageInfo info){
		Debug.Log("Deactivating this Button and Asking for a NewTask");
		isActive = false;
		setTask = false;
		mNView.RPC("InquireNewTask", RPCMode.Server, Network.player);
	}
}
