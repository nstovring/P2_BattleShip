using UnityEngine;
using System.Collections;

public class shipScript : MonoBehaviour {

	public int gridInfluence = 1;
	public int currentRotation = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
