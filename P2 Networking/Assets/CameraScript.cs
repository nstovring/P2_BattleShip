using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	int AngleNum;
	public Transform[] CameraAngles = new Transform[3];
	//public Vector3[] CameraPositions = new Vector3[3];
	//public Vector3[] CameraRotations = new Vector3[3];
	// Use this for initialization
	void Start () {



	}
	public void AssignServerCamera(){
		if(Network.isServer){
			transform.position = CameraAngles[0].position;
			transform.rotation = CameraAngles[0].rotation;
			Camera camera= GetComponent<Camera>();
			camera.orthographic = true;
			camera.orthographicSize = 14.38f;
		}
	}
	public void AssignClientCamera(int playerNum){
		AngleNum = playerNum;
		//This does not work, i know not why, fuck everything
		if(Network.isClient){
		//Debug.Log(playerNum);
			transform.position = CameraAngles[playerNum].position;
			transform.rotation = CameraAngles[playerNum].rotation;
		}
	}
	// Update is called once per frame
	void Update () {
		//print(Network.connections.Length);
		if(Network.isClient){
			//Debug.Log(playerNum);
			//transform.position = CameraAngles[AngleNum].position;
			//transform.rotation = CameraAngles[AngleNum].rotation;
		}
	}
}
