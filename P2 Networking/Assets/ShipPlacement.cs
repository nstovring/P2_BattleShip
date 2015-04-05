using UnityEngine;
using System.Collections;

public class ShipPlacement : MonoBehaviour {
	public GameObject[] ships = new GameObject[4];
	public GameObject[] ghostShips = new GameObject[4];

	private int selectedBoat;

	float mouseWorldPosX;
	private bool placingShip = false;

	public void setPlacingShip(bool placing){
		placingShip = placing;
	}

	public void setSelectedBoat(int boatNum){
		if(boatNum < ships.Length){
		selectedBoat = boatNum;
		}
	}
	public int getSelectedBoat(){
		return selectedBoat;
	}
	
	// Update is called once per frame
	void Update () {
		//To ensure only clients are able to place ships
		//if(Network.isClient){
		PlacingInteraction();
		//}
	}
	void PlacingInteraction(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit, 100)){
			if(hit.transform.tag == "GridSquare" && placingShip){
				//Gather the code from the currently targeted grid and the currently selected ship
				shipScript ShipScript = ships[selectedBoat].GetComponent<shipScript>();
				GridScript gridScript = hit.transform.GetComponent<GridScript>();

				DisplayGhostShip(hit, gridScript);
				//If left mousebutton pressed and the grid is unoccupied
				if(Input.GetMouseButtonDown(0) && !gridScript.getOccupied(ShipScript)){
					ghostShips[selectedBoat].transform.position = new Vector3(50,0,0);

					//Send RPC Call to call the method DeployShip across the network for testing purposes
					DeployShip(hit.transform.position);
					setPlacingShip(false);
				}
			}
		}
	}

	void DisplayGhostShip(RaycastHit hit, GridScript gridScript){
	 	shipScript ShipScript = ghostShips[selectedBoat].GetComponent<shipScript>();
		if(!gridScript.getOccupied(ShipScript)){
		ghostShips[selectedBoat].transform.position = hit.transform.position;
		}
	}

	void DeployShip(Vector3 hit){
		Instantiate(ships[selectedBoat],hit,ghostShips[selectedBoat].transform.rotation);

		//Send positional Data and ship type to server here
	}
}
