using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasController : StateMachine {

	public GameObject[] miniGameButtons = new GameObject[6];
	public Button[] placementbuttons = new Button[7];
	public Button[] attackingButtons = new Button[3];

	// Use this for initialization
	void Start () {
		ShuffleMiniGameButtonsPosition();
	}
	void ShuffleMiniGameButtonsPosition(){
		Vector2[] positions = new Vector2[miniGameButtons.Length];
		for(int i = 0; i < miniGameButtons.Length; i++){
			positions[i] = miniGameButtons[i].GetComponent<RectTransform>().anchoredPosition;
		}
		for(int i = 0; i < miniGameButtons.Length; i++){
			int rng = Random.Range(0, miniGameButtons.Length-1);
			miniGameButtons[i].GetComponent<RectTransform>().anchoredPosition = positions[rng];
			miniGameButtons[rng].GetComponent<RectTransform>().anchoredPosition = positions[i];
		}
	}

	void EnablePlacementButtons(){
			for(int i = 0; i< placementbuttons.Length; i++){
			placementbuttons[i].GetComponent<Image>().enabled = true;
			placementbuttons[i].enabled = true;
			}
	}
	void EnableAttackingButtons(){
		for(int i = 0; i< attackingButtons.Length; i++){
			attackingButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0,300);
		}
	}

	void DisablePlacementButtons(){
		for(int i = 0; i< placementbuttons.Length; i++){
			placementbuttons[i].GetComponent<Image>().enabled = false;
			placementbuttons[i].enabled = false;
		}
	}
	void DisableAttackingButtons(){
		for(int i = 0; i< attackingButtons.Length; i++){
			attackingButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0,900);
		}
	}
	// Update is called once per frame
	void Update () {
		if(Network.isClient && State == 0){
			EnablePlacementButtons();
		}else{
			DisablePlacementButtons();
		}
		if(Network.isServer && State == 2){
			EnableAttackingButtons();
		}else{
			DisableAttackingButtons();
		}
	}
}
