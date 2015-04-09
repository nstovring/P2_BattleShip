using UnityEngine;
using System.Collections;

public class TargetMarker : MonoBehaviour {

	public bool hit = false;
	
	void Update(){

	}

	[RPC]
	void NetWorkChangeColor(){
		MeshRenderer[] renderers=  GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer renderer in renderers){
			renderer.material.color = Color.red;
		}
		//This line, this fucking line is the reason why we do not simply copy and paste code without understanding it first -AARGARRGH
		//GetComponent<NetworkView>().RPC("ChangeColor", RPCMode.OthersBuffered);
		hit = true;
		//GetComponent<BoxCollider>().enabled = false;
	}


	public void ChangeColor(){
		GetComponent<NetworkView>().RPC("NetWorkChangeColor",RPCMode.AllBuffered);
	}
}
