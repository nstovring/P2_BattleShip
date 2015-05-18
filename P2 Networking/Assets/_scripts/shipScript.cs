using UnityEngine;
using System.Collections;

public class shipScript : MonoBehaviour {

	public int Team;
	public int health;
	public int shipType;
	public bool placed = false;
	public int gridInfluence = 1;
	//Rotation of ship is binary, either horizontal or vertical placement
	public int currentRotation = -1;
	float hitTime = 0;
	public GameObject[] destroyedShips = new GameObject[4];
	GameObject stateMachine;
	GameOverController gameOverController; 

<<<<<<< HEAD
	public NetworkView nView;
	StateMachine statemachine;
	void Start() {
=======
	 NetworkView nView;
	 NetworkView sNView;
>>>>>>> origin/Development-NEW

	void Start() {
		nView = GetComponent<NetworkView>();
		stateMachine = GameObject.Find("_StateMachine");
		sNView = stateMachine.GetComponent<NetworkView>();
		gameOverController = stateMachine.GetComponent<GameOverController>();
	}

	void Awake(){

		if(transform.tag == "Ship"){
			LockShip();
		}
	}
	[RPC]
	public void RotateShipLeft(){
		float addRotation = transform.localRotation.y; 
		addRotation = 90;
		transform.RotateAround(transform.position, Vector3.up, addRotation);
		currentRotation *= -1;
	}
	[RPC]
	public void RotateShipRight(){
		float addRotation = transform.localRotation.y; 
		addRotation = -90;
		transform.RotateAround(transform.position, Vector3.up, addRotation);
		currentRotation *= -1;
	}

	public int GetRotation(){
		return currentRotation;
	}
	MeshRenderer[] renderers;
		SpriteRenderer[] sRenderes; 
	void OnNetworkInstantiate(NetworkMessageInfo info) {
		nView = GetComponent<NetworkView>();
		statemachine = GameObject.Find ("_StateMachine").GetComponent<StateMachine> ();
		Debug.Log(nView.viewID + " spawned");
		Debug.Log (Team);
		if (placed == true) {
			statemachine.addShip (Team, this.gameObject);
		}
		//GetComponent<BoxCollider>().enabled = false;
		//if(Network.isClient){
		if(info.sender == Network.connections[0] || info.sender == Network.connections[1] ){
			Team = 1;
		}else if(info.sender == Network.connections[2] || info.sender == Network.connections[3] ){
			Team = 2;
		}else{
			Team = -1;
			}
		//}

		renderers = GetComponentsInChildren<MeshRenderer>();
		sRenderes = GetComponentsInChildren<SpriteRenderer>();
		if(Network.isServer){
			foreach(MeshRenderer renderer in renderers){
				renderer.enabled = false;
			}
			foreach(SpriteRenderer renderer in sRenderes){
				renderer.enabled = false;
			}
		}else{
			foreach(MeshRenderer renderer in renderers){
				renderer.enabled = true;
			}
			foreach(SpriteRenderer renderer in sRenderes){
				renderer.enabled = true;
			}
		}
	}
	void SetTeam(NetworkMessageInfo info){
		if(info.sender == Network.connections[0] || info.sender == Network.connections[1] ){
			Team = 1;
		}else if(info.sender == Network.connections[2] || info.sender == Network.connections[3] ){
			Team = 2;
		}else{
			Team = -1;
		}
	}


	[RPC]
	//Create a destroyed version of itself on the server
	public void Destroyed(){
		//gameOverController.UpdateDestroyedShips(team);

		foreach(MeshRenderer renderer in renderers){
			renderer.enabled = true;
		}
		foreach(SpriteRenderer renderer in sRenderes){
			renderer.enabled = true;
		}
	}
	public bool dead = false;
	// Update is called once per frame
	void Update () {
		if(health <= 0 && transform.tag == "Ship" && !dead){
			Debug.Log("Ship Destroyed");
			sNView.RPC("UpdateDestroyedShips",RPCMode.AllBuffered, Team);
			nView.RPC("Destroyed",RPCMode.AllBuffered);
			dead = true;
		}

		hitTime += Time.deltaTime;

		if(!placed && Input.GetKeyDown(KeyCode.D)){
			RotateShipRight();
		}
		if(!placed && Input.GetKeyDown(KeyCode.A)){
			RotateShipLeft();
		}
	}
	public void LockShip(){
		placed = true;
	}

	public int getGridInfluence(){
		return this.gridInfluence;
	}

	public void setHealth(int health){
		if(this.health<=0){
			this.health = 0;
		}else{
		this.health = health;
		}
	}
	public void Hit(){
		health -= 1;
	}
	public int targetMarkers = 0;
	void OnTriggerStay(Collider others){
		if(others.transform.tag == "TargetMarker"){
			if(others.transform.GetComponent<TargetMarker>().hit == false){
			targetMarkers++;
			Hit();
			others.transform.GetComponent<TargetMarker>().ChangeColor();
			others.transform.GetComponent<TargetMarker>().hit = true;
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.transform.tag == "GridSquare" && Network.isClient){
			other.GetComponent<GridScript>().setOccupied(true);
		}
	}
	void OnTriggerExit(Collider other){
		if(other.transform.tag == "GridSquare" && Network.isClient){
			other.GetComponent<GridScript>().setOccupied(false);
		}
	}


}
