using UnityEngine;
using System.Collections;

public class ShipPlacement : MonoBehaviour {
	public GameObject[] ships = new GameObject[4];
	private int selectedBoat;

	float mouseWorldPosX;
	private bool placingShip = false;
	// Use this for initialization
	void Start () {
	
	}
	
	public void setPlacingShip(bool placing){
		placingShip = placing;
	}

	public void setSelectedBoat(int boatNum){
		//print (boatNum);
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
			//print("Hit something");
			if(hit.transform.tag == "GridSquare" && placingShip){
				//Gather the code from the currently targeted grid and the currently selected ship
				shipScript ShipScript = ships[selectedBoat].GetComponent<shipScript>();
				GridScript gridScript = hit.transform.GetComponent<GridScript>();

				//If left mousebutton pressed and the grid is unoccupied
				if(Input.GetMouseButtonDown(0) && !gridScript.getOccupied(ShipScript)){
					//Send RPC Call to call the method DeployShip across the network
					GetComponent<NetworkView>().RPC("DeployShip",RPCMode.All, hit.transform.position);
					setPlacingShip(false);
				}
				print("Oooh yeah");
			}
		}
	}

	void DisplayGhostShip(){

	}
	[RPC]
	void DeployShip(Vector3 hit){
	 	 Network.Instantiate(ships[selectedBoat],hit,Quaternion.identity,0);
		//clone.transform.name = clone.transform.name +" "+Network.player.ToString;
	}
}
