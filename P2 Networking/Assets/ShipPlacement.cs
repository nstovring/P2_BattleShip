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
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit, 100)){
			//print("Hit something");
			if(hit.transform.tag == "GridSquare" && placingShip){
				if(Input.GetMouseButtonDown(0)){
					DeployShip(hit);
					setPlacingShip(false);
				}
				print("Oooh yeah");
			}
		}
	}

	void DeployShip(RaycastHit hit){
		GameObject clone = Instantiate(ships[selectedBoat],hit.transform.position,Quaternion.identity) as GameObject;
		//clone
	}
}
