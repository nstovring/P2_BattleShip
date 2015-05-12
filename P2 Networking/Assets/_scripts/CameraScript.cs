using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraScript : StateMachine {


	//public int TeamTurn = 0;
	Touch touch;
	public CanvasScaler canvasScaler;
	int AngleNum;
	public Transform[] PlayerAngles = new Transform[3];
	public Transform[] CameraAngles = new Transform[3];
	public void AssignServerCamera(){
		if(Network.isServer){
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			canvasScaler.matchWidthOrHeight = 0f;
			transform.position = CameraAngles[0].position;
			transform.rotation = CameraAngles[0].rotation;
			Camera camera= GetComponent<Camera>();
			camera.orthographic = true;
			camera.orthographicSize = 20.12f;
		}
	}
	//Supposed to give the player a camera view dependant on their id
	public void AssignClientCamera(int playerNum){
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		//ternerary operator double check sometime
		playerNum = playerNum > PlayerAngles.Length ? PlayerAngles.Length: playerNum;
		//This does not work, i know not why, fuck everything -edit omg i figured it out, fuck everything learn to understand networkviews
		if(Network.isClient){
			canvasScaler.matchWidthOrHeight = 0.9f;
			transform.position = PlayerAngles[playerNum].position;
			transform.rotation = PlayerAngles[playerNum].rotation;
			Camera camera= GetComponent<Camera>();
			camera.orthographic = true;
			//camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 20.12f, 0.1f);
			camera.orthographicSize = 12.69f;
		}
	}

	void Update(){
		if(Network.isServer){
			AnimationMovement();
		}
		#if UNITY_EDITOR
		if(Network.isClient){
		UnityMovement();
		}
		#elif UNITY_STANDALONE_WIN
		if(Network.isClient){

			UnityMovement();
		}
		#elif UNITY_ANDROID
			if(Network.isClient){

				//AndroidMovement();
		}
		#endif
	}

	void AnimationMovement(){
		canvasScaler.matchWidthOrHeight = 0.9f;
		transform.position = Vector3.Lerp(transform.position, CameraAngles[TeamTurn].position, 0.1f);
		transform.rotation = Quaternion.Lerp(transform.rotation, CameraAngles[TeamTurn].rotation, 0.1f);
		Camera camera= GetComponent<Camera>();
		//camera.orthographic = true;
		if(TeamTurn >0){
		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 12.56f, 0.1f);
		}else{
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 20.12f, 0.1f);

		}
	}

	void UnityMovement(){
		if(Input.GetAxis("Horizontal")> 0.1f ||Input.GetAxis("Horizontal")< 0.1f ){
			transform.Translate(new Vector3(Input.GetAxis("Horizontal"),0,0));
		}
	}
	void AndroidMovement(){
		//Change this should simply lerp between two or three positions
		if(Input.touchCount > 0)
		{
			touch = Input.touches[0];
			if (touch.phase == TouchPhase.Moved)
			{
				transform.position += Vector3.Lerp(transform.position,new Vector3(touch.deltaPosition.x *-0.5f,0,0),1f);
			}
		}
		transform.position += Vector3.Lerp(transform.position,new Vector3(touch.deltaPosition.x *-0.5f,0,0),1f);
	}
//	transform.position += Vector3.Lerp(transform.position,new Vector3(touch.deltaPosition.x *-0.5f,0,0),1f);
	//works now because of reasons
}
