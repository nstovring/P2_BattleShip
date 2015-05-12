using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasController : StateMachine {

	public GameObject miniGameButtonParent;
	public GameObject placementButtonParent;
	public GameObject[] miniGameButtons = new GameObject[6];
	public Button[] placementbuttons = new Button[7];
	public Button[] attackingButtons = new Button[3];
	public Text[] scoreText = new Text[2];
	public GameObject teamText;

	// Use this for initialization
	void Start () {
		ShuffleMiniGameButtonsPosition();
	}
	void ShuffleMiniGameButtonsPosition(){
		Vector2[] positions = new Vector2[miniGameButtons.Length];
		for(int i = 0; i < miniGameButtons.Length; i++){
			positions[i] = miniGameButtons[i].GetComponent<RectTransform>().anchoredPosition;
		}
		//Shuffle positions
		for(int i = 0; i < miniGameButtons.Length; i++){
			int rng = Random.Range(i, miniGameButtons.Length);
			Vector2 tempPosition = positions[i];
			positions[i] = positions[rng];
			positions[rng] = tempPosition;

		}
		for(int i = 0; i < miniGameButtons.Length; i++){
			miniGameButtons[i].GetComponent<RectTransform>().anchoredPosition = positions[i];
		}
	}
	void EnableMiniGameButtons(){
		Vector2 pPosition = miniGameButtonParent.GetComponent<RectTransform>().anchoredPosition; 
		pPosition = Vector2.Lerp(pPosition, new Vector2(0,0),0.1f);
		miniGameButtonParent.GetComponent<RectTransform>().anchoredPosition = pPosition;
	}

	void DisableMiniGameButtons(){
		miniGameButtonParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,800);
	}

	void EnablePlacementButtons(){
		placementButtonParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
	}
	void EnableAttackingButtons(){
		for(int i = 0; i< attackingButtons.Length; i++){
			attackingButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0,300);
		}
	}

	void DisablePlacementButtons(){
		placementButtonParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,800);

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
		if(Network.isClient && State == 1){
			EnableMiniGameButtons();
		}else{
			DisableMiniGameButtons();
		}

		if(Network.isServer && State == 2){
			EnableAttackingButtons();
		}else{
			DisableAttackingButtons();
		}
	}
}
