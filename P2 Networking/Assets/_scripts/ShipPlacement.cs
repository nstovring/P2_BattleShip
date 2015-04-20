using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipPlacement : MonoBehaviour {
	public GameObject[] ships = new GameObject[4];
	public GameObject[] ghostShips = new GameObject[4];
	public Button[] buttons = new Button[6];
	//Amount of ships available
	private int[] availableShips = {1,2,1,1,1};

	private int gridLayer = 1<< 8;
	//interesting stuff gotta investigate later
	//gridLayer = ~gridLayer;

	private int selectedShip;

	private bool placingShip = false;

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
	
	// Update is called once per frame
	void Update () {
		//Disable or enable the rotate buttons depending on ship placement
		buttons[4].interactable = !placingShip ? false: true;
		buttons[5].interactable = !placingShip ? false: true;
		//To ensure only clients are able to place ships
	#if UNITY_EDITOR
		//if(Network.isClient){
		Placement();
		//}
	#elif UNITY_STANDALONE_WIN
		//if(Network.isClient){
		Placement();
		//}
	#elif UNITY_ANDROID
		if(Network.isClient){
		AndroidPlacement();
		}
	#endif
	}
	RaycastHit hit;
	void AndroidPlacement(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray,out hit, 100,gridLayer)){
			if(hit.transform.tag == "GridSquare" && placingShip){
				//Gather the code from the currently targeted grid and the currently selected ship
//				shipScript ShipScript = ships[selectedShip].GetComponent<shipScript>();
				//GridScript gridScript = hit.transform.GetComponent<GridScript>();
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

				DisplayGhostShip(pcHit);
				//If left mousebutton pressed and the grid is unoccupied
				if(Input.GetMouseButtonDown(0) && !gridScript.getOccupied(ShipScript)){
					//Send RPC Call to call the method DeployShip across the network for testing purposes
					DeployShip();
				}
			}
		}
	}

	void DisplayGhostShip(RaycastHit hit){
	 	shipScript ShipScript = ghostShips[selectedShip].GetComponent<shipScript>();
		GridScript gridScript = hit.transform.GetComponent<GridScript>();

		if(!gridScript.getOccupied(ShipScript)){
		ghostShips[selectedShip].transform.position = hit.transform.position;
		}
	}
	public void substractShip(int num, int _selectedShip){
		if(availableShips[_selectedShip] > 0){
			availableShips[_selectedShip] -= num;
		}
	}


	public void DeployShip(){
		Instantiate(ships[selectedShip],ghostShips[selectedShip].transform.position,ghostShips[selectedShip].transform.rotation);
		ghostShips[selectedShip].transform.position = new Vector3(50,0,0);
		//Reduce amount of ships Avaliable
		substractShip(1,selectedShip);
		//availableShips[selectedShip] -= 1;
		//Disable Shipbutton if there are no more ships avaliable
		buttons[selectedShip].interactable = availableShips[selectedShip] == 0 ? false : true;
		//Player is no longer placing a ship
		setPlacingShip(false);
	}
}
