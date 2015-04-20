using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour {

	public CanvasScaler canvasScaler;
	int AngleNum;
	public Transform[] CameraAngles = new Transform[3];

	public void AssignServerCamera(){
		if(Network.isServer){
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			canvasScaler.matchWidthOrHeight = 0f;
			transform.position = CameraAngles[0].position;
			transform.rotation = CameraAngles[0].rotation;
			Camera camera= GetComponent<Camera>();
			camera.orthographic = true;
			camera.orthographicSize = 13.12f;
		}
	}
	//Supposed to give the player a camera view dependant on their id
	public void AssignClientCamera(int playerNum){
		Screen.orientation = ScreenOrientation.Portrait;
		//ternerary operator double check sometime
		playerNum = playerNum > CameraAngles.Length ? CameraAngles.Length: playerNum;
		//This does not work, i know not why, fuck everything -edit omg i figured it out, fuck everything learn to understand networkviews
		if(Network.isClient){
			canvasScaler.matchWidthOrHeight = 0.9f;
			transform.position = CameraAngles[playerNum].position;
			transform.rotation = CameraAngles[playerNum].rotation;
			Camera camera= GetComponent<Camera>();
			camera.orthographic = true;
			camera.orthographicSize = 16.8f;
		}
	}
	//works now because of reasons
}
