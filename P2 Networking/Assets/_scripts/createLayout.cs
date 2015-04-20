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
	public bool create = false;
	public bool destroy = false;
	// Use this for initialization
	void Start () {
		//CreateLayout();

	}
	void Awake () {
		//CreateLayout();
		
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
		if(create){
			CreateLayout();
			create = false;
		}
		if(destroy){
			Transform[] children= transform.GetComponentsInChildren<Transform>();
			foreach (Transform child in children){
				DestroyImmediate(child.gameObject);
			}
			destroy = false;
		}
		//StartCoroutine(MakeGridsDistinguishable());
	}
}
