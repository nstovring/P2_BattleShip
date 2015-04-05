using UnityEngine;
using System.Collections;

public class TargetMarker : MonoBehaviour {

	public bool hit = false;

	[RPC]
	public void ChangeColor(){
		MeshRenderer[] renderers=  GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer renderer in renderers){
			renderer.material.color = Color.red;
		}

		GetComponent<NetworkView>().RPC("ChangeColor", RPCMode.OthersBuffered);
		hit = true;
		//GetComponent<BoxCollider>().enabled = false;
	}
}
