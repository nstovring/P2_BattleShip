using UnityEngine;
using System.Collections;
// This class Creates the grid layout 
//[ExecuteInEditMode]
public class createLayout : MonoBehaviour {
	public GameObject grid;
	public int rows = 10;
	public int collumns = 10;
	// Use this for initialization
	void Start () {
		//A nested for loop creates a grid dependant on the the scale of the grid gameObject 
		//and the amount of rows and collumns
		float posX = gameObject.transform.position.x;
		float posZ = gameObject.transform.position.z;
		for(int i = 0; i < rows; i++){
			posZ += grid.transform.localScale.x;
			for(int j = 0; j < collumns; j++){
				GameObject clone2 = Instantiate(grid,new Vector3(posX,0,posZ),Quaternion.identity) as GameObject;
				clone2 .transform.parent = gameObject.transform;
				clone2.transform.name = "Position: " + i+ ","+ j; 
				posZ += grid.transform.localScale.z;
			}
			posX += grid.transform.localScale.x;
			posZ = gameObject.transform.position.z;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
