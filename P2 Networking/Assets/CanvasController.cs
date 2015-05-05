using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasController : StateMachine {

	public Button[] placementbuttons = new Button[7];
	public Button[] attackingButtons = new Button[3];

	// Use this for initialization
	void Start () {
	
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
			//attackingButtons[i].GetComponent<Image>().enabled = true;
			//attackingButtons[i].enabled = true;
		}
	}
	// Update is called once per frame
	void Update () {
		if(Network.isClient && State == 0){
			EnablePlacementButtons();
		}
		if(Network.isServer && State == 2){
			EnableAttackingButtons();
		}
	}
}
