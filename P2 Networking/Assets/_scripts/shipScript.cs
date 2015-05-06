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
	NetworkPlayer creator;
	void Start() {

	}

	void Awake(){

		if(transform.tag == "Ship"){
			LockShip();
		}
	}

	public void RotateShipLeft(){
		float addRotation = transform.localRotation.y; 
		addRotation+= 90;
		transform.Rotate(new Vector3(0,addRotation,0));
		currentRotation *= -1;
	}
	public void RotateShipRight(){
		float addRotation = transform.localRotation.y; 
		addRotation-= 90;
		transform.Rotate(new Vector3(0,addRotation,0));
		currentRotation *= -1;
	}

	public int GetRotation(){
		return currentRotation;
	}
	void OnNetworkInstantiate(NetworkMessageInfo info) {
		nView = GetComponent<NetworkView>();
		Debug.Log(nView.viewID + " spawned");
		creator = info.sender;
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
		
		if(Network.isServer && health > 0){
			//Renderer[] renderers = GetComponentsInChildren<Renderer>();
			foreach(MeshRenderer renderer in renderers){
				renderer.enabled = false;
			}
		}else{
			foreach(MeshRenderer renderer in renderers){
				renderer.enabled = true;
			}
		}
	}

	[RPC]
	//Create a destroyed version of itself on the server
	public void Destroyed(){

		Network.Instantiate(destroyedShips[shipType],transform.position,transform.rotation,0);
		Network.RemoveRPCs(nView.viewID);
		Network.Destroy(gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		if(health <= 0 && transform.tag == "Ship"){
			Debug.Log("Ship Destroyed");
			nView.RPC("Destroyed",RPCMode.AllBuffered);
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
		if(hitTime >= 0.5f){
		//if(creator == Network.player){
			#if UNITY_ANDROID
			Handheld.Vibrate();
			#endif
		//}
		setHealth(health-=1);
			hitTime = 0;
		}
	}


	void OnTriggerEnter(Collider other){
		if(other.transform.tag == "GridSquare" && Network.isClient){
			other.GetComponent<GridScript>().setOccupied(true);
		}
		if(other.transform.tag == "TargetMarker"){
			other.transform.GetComponent<TargetMarker>().ChangeColor();
			Hit ();
		}
	}
}
