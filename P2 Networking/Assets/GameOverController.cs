using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {

	//
	public int[] destroyedShips = {0,0};
	public GameObject gameOverDisplay;
	public int destroyedShipMin = 5;
	[RPC]
	public void UpdateDestroyedShips(int team){
		//team --;
		if(team == 1){
		destroyedShips[0]++;
		}
		if(team == 2){
		destroyedShips[1]++;
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(destroyedShips[0] >= destroyedShipMin){
			Debug.Log("Team 2 Wins!");
		}
		if(destroyedShips[1] >= destroyedShipMin){
			Debug.Log("Team 1 Wins!");
		}
	}
}
