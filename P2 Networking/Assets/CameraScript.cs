﻿using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	int AngleNum;
	public Transform[] CameraAngles = new Transform[3];

	public void AssignServerCamera(){
		if(Network.isServer){
			transform.position = CameraAngles[0].position;
			transform.rotation = CameraAngles[0].rotation;
			Camera camera= GetComponent<Camera>();
			camera.orthographic = true;
			camera.orthographicSize = 11.18f;
		}
	}
	//Supposed to give the player a camera view dependant on their id
	public void AssignClientCamera(int playerNum){
		//ternerary operator double check sometime
		playerNum = playerNum > CameraAngles.Length ? CameraAngles.Length: playerNum;
		//This does not work, i know not why, fuck everything -edit omg i figured it out, fuck everything learn to understand networkviews
		if(Network.isClient){
			transform.position = CameraAngles[playerNum].position;
			transform.rotation = CameraAngles[playerNum].rotation;
		}
	}
	//works now because of reasons
}
