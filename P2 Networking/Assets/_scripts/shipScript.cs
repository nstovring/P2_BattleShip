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

	public NetworkView nView;
	void Start() {

	}

	void Awake(){

		if(transform.tag == "Ship"){
			LockShip();
		}
	}

	public void RotateShipLeft(){
		float addRotation = transform.localRotation.y; 
		addRotation = 90;
		transform.RotateAround(transform.position, Vector3.up, addRotation);
		currentRotation *= -1;
	}
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
		Debug.Log(nView.viewID + " spawned");
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

	[RPC]
	//Create a destroyed version of itself on the server
	public void Destroyed(){
		foreach(MeshRenderer renderer in renderers){
			renderer.enabled = true;
		}
		foreach(SpriteRenderer renderer in sRenderes){
			renderer.enabled = true;
		}
		//Network.Instantiate(destroyedShips[shipType],transform.position,transform.rotation,0);
		//Network.RemoveRPCs(nView.viewID);
		//Network.Destroy(gameObject);

	}
	bool dead = false;
	// Update is called once per frame
	void Update () {
		if(health <= 0 && transform.tag == "Ship" && !dead){
			Debug.Log("Ship Destroyed");
			nView.RPC("Destroyed",RPCMode.AllBuffered);
			dead = true;
			//Destroyed();
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
}
