using UnityEngine;
using System.Collections;

public class GridScript : MonoBehaviour {
	public bool occupied = false;
	private int row;
	private int collumn;
	TextMesh[] childText;
	createLayout createLayout;
	GridScript[,] grids;
	// Update is called once per frame
	void Start(){
		//Get the createLayout script from the parent of this object
		createLayout = transform.parent.GetComponent<createLayout>();
		//Get the grid class from the parent
		grids = createLayout.grids;
	}

	void Update () {
		//If the grid is occupied change the color to red
		if(occupied){
			//ChangeColor(Color.red);
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
	public void ChangeColor(Color color ){
		MeshRenderer renderer = GetComponent<MeshRenderer>();
		renderer.material.color = color;
	}
	public bool rotationPossible(shipScript shipScript){
		for(int i = 0; i < shipScript.getGridInfluence(); i++){
		if(!grids[row- i,collumn].getOccupied() && !grids[row + i,collumn].getOccupied() && !grids[row,collumn - i].getOccupied() && !grids[row,collumn +i].getOccupied()){
			return true;
		}
		}
		return false;
	}
	
	void OnCollisionExit(Collision info){
		Debug.Log ("Something left me");
		if(info.transform.tag == "Ship"){
			occupied = false;
		}
	}

	//Get occupied checks wether or not a ship is able to be placed at a position
	public bool getOccupied(shipScript shipScript){

		//temporary boolean
		bool tempOccupied = true;
		//Loop which iterates the amount of grids which the ship would cover
		for(int i = 0; i < shipScript.getGridInfluence(); i++){
			//relative to this grids position in the grids array, this if statement will check wether or not the grid below or above and 
			//next to it is occupied as well as checking if the grid even exists

			if(shipScript.GetRotation() == 1 && grids[row- i,collumn] != null && grids[row + i,collumn] != null){
				// If the grids on this axis are unoccupied return false else true
				if(!grids[row- i,collumn].getOccupied() && !grids[row + i,collumn].getOccupied()){
					tempOccupied = false;
				}else{
					return tempOccupied = true;
				}
			} 
			if(shipScript.GetRotation() == -1 && grids[row,collumn - i] != null && grids[row,collumn +i] != null){
				// If the grids on this axis are unoccupied return false else true
				if(!grids[row,collumn - i].getOccupied() && !grids[row,collumn +i].getOccupied()){
					tempOccupied = false;
				}else{
					return tempOccupied = true;
				}
			}
		}
		//If no if statements go through simply return tempOccupied
		return tempOccupied;
	}
}
