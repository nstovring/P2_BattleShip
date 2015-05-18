using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipPlacement : StateMachine {
	public GameObject[] ships = new GameObject[4];
	public GameObject[] ghostShips = new GameObject[4];
	public GameObject[] netGhostShips = new GameObject[4];
	StateMachine statemachine;

	public Button[] buttons = new Button[6];
	public bool ready = false;
	bool readySet = false;
	//Amount of ships available
	public int[] availableShips = {1,2,1,1};
//	shipScript selectedShipScript;
	private int gridLayer = 1<< 8;
	GridScript gridScript;
	RaycastHit hit;
	private int selectedShip;
	bool allShipsPlaced = false;
	private bool placingShip = false;
	public LayerMask myLayerMask;
	[RPC]
	public bool GetReady(){
		return ready;
	}
	public void setPlacingShip(bool placing){
		placingShip = placing;
	}

	public void setSelectedBoat(int shipNum){
		if(shipNum < ships.Length && availableShips[shipNum]>0){
			selectedShip = shipNum;
		}
	}
	public int getSelectedBoat(){
		return selectedShip;
	}

	public void setReady(bool _ready){
		ready = _ready;
	}

	void Start(){
		//Instantiate Networked Ghost Ships
		nView = GameObject.Find("_StateMachine").GetComponent<NetworkView>();
		//statemachine = GameObject.Find ("_StateMachine").GetComponent<StateMachine> ();
		/*for(int i = 0; i < ships.Length; i++){
			netGhostShips[i] = Network.Instantiate(ghostShips[i],transform.position,Quaternion.identity,0) as GameObject;
		}*/
	}

	public void SpawnGhostShips(){
		for(int i = 0; i < ships.Length; i++){
			netGhostShips[i] = Network.Instantiate(ghostShips[i],new Vector3(50,0,0),Quaternion.identity,0) as GameObject;
		}
	}

	// Update is called once per frame
	void Update () {
		for(int i= 0; i < availableShips.Length; i++){
			if(availableShips[i] > 0){
				break;
			}else{
				allShipsPlaced = true;
			}
		}
		//Disable or enable shiplacement button depending on allshipsPlaced bool
		//buttons[6].interactable = allShipsPlaced ? false: true;

		if(Network.isServer){
			for(int i = 0; i< buttons.Length; i++){
				buttons[i].GetComponent<Image>().enabled = false;
			}
		}

		if(ready && !readySet){
			//Disable place ship button
			buttons[6].interactable = false;
			Debug.Log("I am ready");
			//Call CheckforReady method on the StateMachine
			nView.RPC("CheckForReady", RPCMode.AllBuffered,1);
			//Stop the ready boolean from turning true again
			availableShips[0] = 1;
			readySet = true;
		}
		//Rotate buttons only interactable if a ship is currently being placed
		buttons[4].interactable = !placingShip ? false: true;
		buttons[5].interactable = !placingShip ? false: true;
		//buttons[0].interactable = ghostShipActive && selectedShip == 0 ? false: true;
		//buttons[1].interactable = ghostShipActive && selectedShip == 1 ? false: true;
		//buttons[2].interactable = ghostShipActive && selectedShip == 2 ? false: true;
		//buttons[3].interactable = ghostShipActive && selectedShip == 3 ? false: true;
		//To ensure only clients are able to place ships
	#if UNITY_EDITOR
		if(Network.isClient && State == 0){
		Placement();
		}
	#elif UNITY_STANDALONE_WIN
		if(Network.isClient && State == 0){
		Placement();
		}
	#elif UNITY_ANDROID
		if(Network.isClient && State == 0){
		AndroidPlacement();
		}
	#endif
	}

	void AndroidPlacement(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray,out hit, 100,gridLayer)){
			if(hit.transform.tag == "GridSquare" && placingShip){
				//Gather the code from the currently targeted grid and the currently selected ship
				DisplayGhostShip(hit);
			}
		}
	}

	void Placement(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit pcHit;
		if (Physics.Raycast(ray,out pcHit, 100,gridLayer)){
			if(pcHit.transform.tag == "GridSquare" && placingShip){
				//Gather the code from the currently targeted grid and the currently selected ship
				shipScript ShipScript = ships[selectedShip].GetComponent<shipScript>();
				GridScript gridScript = pcHit.transform.GetComponent<GridScript>();
	//			selectedShipScript = ShipScript;
				DisplayGhostShip(pcHit);
				//If left mousebutton pressed and the grid is unoccupied
				if(Input.GetMouseButtonDown(0) && !gridScript.getOccupied(ShipScript)){
					//Send RPC Call to call the method DeployShip across the network for testing purposes
					DeployShip();
					Debug.Log("ship " +ShipScript.shipType + " send");
				}
			}
		}
	}
	bool ghostShipActive = false;
	void DisplayGhostShip(RaycastHit hit){
		ghostShipActive = true;
	 	shipScript ShipScript = netGhostShips[selectedShip].GetComponent<shipScript>();
		GridScript gridScript = hit.transform.GetComponent<GridScript>();
		//GameObject clone = Network.Instantiate()
		if(gridScript.rotationPossible(ShipScript)&& RotateLeft){
			//netGhostShips[selectedShip].transform.RotateAround(transform.position, Vector3.up,90);
			//ShipScript.RotateShipLeft();
			RotateLeft = false;
		}
		if(gridScript.rotationPossible(ShipScript) && RotateRight){
			//netGhostShips[selectedShip].transform.RotateAround(transform.position, Vector3.up,-90);
			//ShipScript.RotateShipRight();
			RotateRight = false;
		}
		if(!gridScript.getOccupied(ShipScript)){
		netGhostShips[selectedShip].transform.position = hit.transform.position;
		}
	}
	public bool RotateRight;
	public bool RotateLeft;

	public void substractShip(int num, int _selectedShip){
		if(availableShips[_selectedShip] > 0){
			availableShips[_selectedShip] -= num;
		}
	}

	public void RotateShipLeft(){
		shipScript ShipScript = netGhostShips[selectedShip].GetComponent<shipScript>();
		netGhostShips[selectedShip].GetComponent<NetworkView>().RPC("RotateShipLeft",RPCMode.AllBuffered);
		//ShipScript.RotateShipLeft();
		RotateLeft = true;
	}
	public void RotateShipRight(){
		shipScript ShipScript = netGhostShips[selectedShip].GetComponent<shipScript>();
		netGhostShips[selectedShip].GetComponent<NetworkView>().RPC("RotateShipRight",RPCMode.AllBuffered);
		//ShipScript.RotateShipRight();
		RotateRight = true;
	}

	public void DeployShip(){
		if(placingShip){
		//statemachine.addShipCall(int.Parse(Network.player.ToString()),ships[selectedShip]);
		//nView.RPC("addShip",RPCMode.AllBuffered,int.Parse(Network.player.ToString()),ships[selectedShip]);
		//Instantiate a ship at the position of the ghost ship
		Network.Instantiate(ships[selectedShip],netGhostShips[selectedShip].transform.position,netGhostShips[selectedShip].transform.rotation,0);
		//Move the ghost ship away
		netGhostShips[selectedShip].transform.position = new Vector3(50,0,0);
		//Reduce amount of ships Avaliable
		substractShip(1,selectedShip);
		//Disable Shipbutton if there are no more ships avaliable
		buttons[selectedShip].interactable = availableShips[selectedShip] == 0 ? false : true;
		//Player is no longer placing a ship
		setPlacingShip(false);
		ghostShipActive = false;
		//Reset the selectedShip Variable
		}
	}
}
