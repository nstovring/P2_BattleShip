using UnityEngine;
using System.Collections;

public class GridScript : MonoBehaviour {
	public bool occupied = false;
	private int row;
	private int collumn;
	TextMesh[] childText;

	// Update is called once per frame
	void Update () {
		if(occupied){
			ChangeColor(Color.red);
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
	
	public bool getOccupied(shipScript shipScript){
		//Get the createLayout script from the parent of this object
		createLayout createLayout = transform.parent.GetComponent<createLayout>();
		GridScript[,] grids = createLayout.grids;
		bool tempOccupied = true;
		for(int i = 0; i < shipScript.getGridInfluence(); i++){
			//relative to this grids position in the grids array, this if statement will check wether or not the grid below or above and 
			//next to it is occupied as well as checking if the grid even exists
			if(shipScript.GetRotation() == 1){
				if(!grids[row- i,collumn].getOccupied() && !grids[row + i,collumn].getOccupied()){
					tempOccupied = false;
				}else{
					return tempOccupied = true;
				}
			}else if(shipScript.GetRotation() == -1){
				if(!grids[row,collumn - i].getOccupied() && !grids[row,collumn +i].getOccupied()){
					tempOccupied = false;
				}else{
					return tempOccupied = true;
				}
			}
		}
		return tempOccupied;
	}
}
