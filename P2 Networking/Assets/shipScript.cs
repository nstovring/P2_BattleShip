using UnityEngine;
using System.Collections;

public class shipScript : MonoBehaviour {

	public bool placed = false;
	public int gridInfluence = 1;
	//Rotation of ship is binary, either horizontal or v ertical placement
	public int currentRotation = -1;
	// Use this for initialization
	void Start () {
		currentRotation = -1;
		if(transform.tag == "Ship"){
			LockShip();
		}
	}
	void Awake(){
		if(transform.tag == "Ship"){
			LockShip();
		}
	}

	void RotateShip(){
		float addRotation = transform.localRotation.y; 
		addRotation+= 90;
		transform.Rotate(new Vector3(0,addRotation,0));
		currentRotation *= -1;
	}

	public int GetRotation(){
		return currentRotation;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.KeypadEnter) && transform.tag == "Ship"){
			placed = true;
		}

		if(!placed && Input.GetKeyDown(KeyCode.D) || !placed && Input.GetKeyDown(KeyCode.A)){
			RotateShip();
		}
	}
	public void LockShip(){
		placed = true;
	}

	public int getGridInfluence(){
		return this.gridInfluence;
	}

	void OnTriggerEnter(Collider other){
		if(other.transform.tag == "GridSquare"){
			other.GetComponent<GridScript>().setOccupied(true);
			Debug.Log ("This nigga is occupied");
		}
	}
}
