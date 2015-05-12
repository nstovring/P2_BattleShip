using UnityEngine;
using System.Collections;

public class TargetMarker : MonoBehaviour {

	public Transform hitShip;
	public bool hit = false;
	public GameObject explosion;
	void Update(){
		/*if(hit && hitShip != null){
			GetComponent<BoxCollider>().enabled = false;
			//hitShip.GetComponent<shipScript>().Hit();
			Debug.Log("A ship has been hit");
			hit =false;
		}*/

	}

	[RPC]
	void NetWorkChangeColor(){
		MeshRenderer[] renderers=  GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer renderer in renderers){
			renderer.material.color = Color.red;
		}
		//This line, this fucking line is the reason why we do not simply copy and paste code without understanding it first -AARGARRGH infinite loop action
		hit = true;
	}

	//bool hitDone = false;

	public void ChangeColor(){
		GetComponent<NetworkView>().RPC("NetWorkChangeColor",RPCMode.AllBuffered);
		Network.Instantiate(explosion,transform.position,Quaternion.identity,0);
	}
	

}
