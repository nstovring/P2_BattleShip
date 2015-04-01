using UnityEngine;
using System.Collections;

public class GridScript : MonoBehaviour {
	public bool occupied = false;
	private int row;
	private int collumn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(occupied){
			ChangeColor();
		}
	
	}
	public void setRowCollumn(int row, int collumn){
		this.row = row;
		this.collumn = collumn;
	}


	public void setOccupied(bool occupied){
		this.occupied = occupied;
	}

	public bool getOccupied(){
		return occupied;
	}
	public void ChangeColor(){
		GetComponent<MeshRenderer>().material.color = Color.red;
	}

	public bool getOccupied(shipScript shipScript){
		//Get the createLayout script from the parent of this object
		createLayout createLayout = transform.parent.GetComponent<createLayout>();
		GridScript[,] grids = createLayout.grids;
		bool tempOccupied = false;
		for(int i = 0; i < shipScript.getGridInfluence(); i++){
			//relative to this grids position in the grids array, this if statement will check wether or not the grid below or above and next to it is occupied
			if(shipScript.currentRotation == 0){
				if(!grids[row,collumn+i].getOccupied() && !grids[row + i,collumn].getOccupied()){
					tempOccupied = false;
				}
			}else if(shipScript.currentRotation == 1){
				if(!grids[row,collumn - i].getOccupied() && !grids[row - i,collumn].getOccupied()){
					tempOccupied = false;
				}
			}else{
				tempOccupied = true;
			}
			/*if(!grids[row,collumn+i].getOccupied() && !grids[row + i,collumn].getOccupied() && 
			   !grids[row,collumn - i].getOccupied() && !grids[row - i,collumn].getOccupied()){
				grids[row,collumn - i].ChangeColor();
				grids[row,collumn + i].ChangeColor();
				grids[row +i,collumn].ChangeColor();
				grids[row - i,collumn].ChangeColor();
				Debug.Log ("Geting here?");
				tempOccupied = false;
			}else{
				tempOccupied = true;
			}*/
		}

		return tempOccupied;
	}
}
