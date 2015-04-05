using UnityEngine;
using System.Collections;
// This class Creates the grid layout 
//[ExecuteInEditMode]
public class createLayout : MonoBehaviour {
	public GameObject grid;
	public int rows = 20;
	public int collumns = 40;
	public GridScript[,] grids;
	public bool gridToSquareSize = true;
	// Use this for initialization
	void Start () {
		//CreateLayout();

		//Debug.Log(grids[10,10].ToString());
	}
	public void CreateLayout(){

		float offsetSize = gridToSquareSize ? grid.transform.localScale.z : 1;
		// A jagged array containg every single gridScript ingame
		grids = new GridScript[rows,collumns];
		//A nested for loop creates a grid dependant on the the scale of the grid gameObject 
		//and the amount of rows and collumns
		float posX = gameObject.transform.position.x;
		float posZ = gameObject.transform.position.z;
		for(int i = 0; i < rows; i++){
			//posZ += grid.transform.localScale.x;
			for(int j = 0; j < collumns; j++){
				GameObject clone2 = Instantiate(grid,new Vector3(posX,0,posZ),Quaternion.identity) as GameObject;
				clone2 .transform.parent = gameObject.transform;
				clone2.transform.name = "Position: " + (i+1)+ ","+ (j+1); 
				//Assigns the grid a position in the jagged array in a matrix like fashion
				grids[i, j] = clone2.GetComponent<GridScript>();
				grids[i, j].setRowCollumn(i,j);
				posZ += offsetSize;
			}
			posX += offsetSize;
			posZ = gameObject.transform.position.z;
		}
	}

	// Update is called once per frame
	void Update () {
		//StartCoroutine(MakeGridsDistinguishable());
	}
}
