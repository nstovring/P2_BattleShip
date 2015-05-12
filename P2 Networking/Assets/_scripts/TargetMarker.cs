using UnityEngine;
using System.Collections;

public class TargetMarker : MonoBehaviour {

	Transform hitShip;
	public bool hit = false;
	public GameObject explosion;
	void Update(){
		if(hit){
			hitShip.GetComponent<shipScript>().Hit();
			hit =false;
		}

	}

	[RPC]
	void NetWorkChangeColor(){
		MeshRenderer[] renderers=  GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer renderer in renderers){
			renderer.material.color = Color.red;
		}
		//This line, this fucking line is the reason why we do not simply copy and paste code without understanding it first -AARGARRGH infinite loop action
		//GetComponent<NetworkView>().RPC("ChangeColor", RPCMode.OthersBuffered);
		hit = true;
		//GetComponent<BoxCollider>().enabled = false;
	}

	bool hitDone = false;

	public void ChangeColor(){
		GetComponent<NetworkView>().RPC("NetWorkChangeColor",RPCMode.AllBuffered);
		Network.Instantiate(explosion,transform.position,Quaternion.identity,0);
	}

	void OnTriggerStay(Collider col){
		if(col.transform.tag == "Ship" && !hit && !hitDone){
			hitShip = col.transform;
			ChangeColor();
			hitDone = true;
			hit = true;
		}
	}

}
