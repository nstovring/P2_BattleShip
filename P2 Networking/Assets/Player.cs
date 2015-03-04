using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private float lastSynchonizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	public float speed = 10f;
	
	void Update()
	{
		if(GetComponent<NetworkView>().isMine){
		InputMovement();
		}
	}
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){
		Vector3 syncPosition = Vector3.zero;

		if(stream.isWriting){
			syncPosition = GetComponent<Rigidbody>().position;
			stream.Serialize(ref syncPosition);
		}else{
			stream.Serialize(ref syncPosition);

			syncTime = 0f;
			syncDelay = Time.time - lastSynchonizationTime; 
			lastSynchonizationTime = Time.time;

			syncStartPosition = GetComponent<Rigidbody>().position;
			syncEndPosition = syncPosition;
			//rigidbody.position = syncPosition;
		}
	}
	
	void InputMovement()
	{
		if(Input.GetAxis("Horizontal") > 0.1 ||Input.GetAxis("Horizontal") < 0.1 || Input.GetAxis("Vertical") > 0.1|| Input.GetAxis("Vertical") < 0.1){
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"))* speed * Time.deltaTime);
		}
	}
}
