using UnityEngine;
using System.Collections;

public class TargetMarker : MonoBehaviour {

	public bool hit = false;
	
	void Update(){
		/*MeshRenderer[] renderers=  GetComponentsInChildren<MeshRenderer>();

		if(hit){
		foreach(MeshRenderer renderer in renderers){
			//renderer.material.shader = Shader.Find("Transparent/Diffuse");
			Color color = renderer.material.color;
			color = Color.red;
			color.a = 50;
			renderer.material.color = color;
			}
		}*/
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
