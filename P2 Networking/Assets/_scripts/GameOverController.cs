using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {

	//
	public int[] destroyedShips = {0,0};
	public GameObject gameOverDisplay;
	public int destroyedShipMin = 1;
	NetworkView nView;
	int player;
	[RPC]
	public void UpdateDestroyedShips(int team){
		if(team == 1){
		destroyedShips[0]++;
		}
		if(team == 2){
		destroyedShips[1]++;
		}
		nView.RPC("checkWin",RPCMode.AllBuffered);
	}
	// Use this for initialization
	void Start () {
		nView = GetComponent<NetworkView> ();
	}

	void OnConnectedToServer(){
		player = int.Parse (Network.player.ToString ());
	}
	
	// Update is called once per frame
	void Update () {

	}
	[RPC]
	public void checkWin(){
		if(Network.isClient){
			if(destroyedShips[0] >= destroyedShipMin){
				gameOverDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
				if(player == 1 || player == 2){
					gameOverDisplay.GetComponentInChildren<Text>().text = "YOU LOSE";
					Debug.Log("Team 1 Wins!");
				}
				else if(player == 3|| player == 4){
					gameOverDisplay.GetComponentInChildren<Text>().text = "YOU WIN";
					Debug.Log("Team 2 Wins!");
				}
			}
			else if(destroyedShips[1] >= destroyedShipMin){
				gameOverDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
				if(player == 1 || player == 2){
					gameOverDisplay.GetComponentInChildren<Text>().text = "YOU WIN";
					Debug.Log("Team 1 Wins!");
				}
				else if(player == 3 || player == 4){
					gameOverDisplay.GetComponentInChildren<Text>().text = "YOU LOSE";
					Debug.Log("Team 2 Wins!");
				};
			}
		}
		if (Network.isServer) {
			if(destroyedShips[0] >= destroyedShipMin || destroyedShips[1] >= destroyedShipMin){
				gameOverDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
				gameOverDisplay.GetComponentInChildren<Text>().text = "GAME OVER";
			}
		}
	}
}
