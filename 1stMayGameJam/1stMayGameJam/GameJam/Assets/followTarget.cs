using UnityEngine;
using System.Collections;

public class followTarget : MonoBehaviour {
	public Rigidbody rb;
	public GameObject player;
	public float speed;
	float stunTime = 0;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {

	
	}
	void FixedUpdate() {
		stunTime += Time.deltaTime *1;

		if(stunTime >= 3){
		Move();
		}
		//rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
	}

	public void Bounce(){
		stunTime = 0;
		rb.AddForce((transform.position + transform.forward)*-10,ForceMode.Force);
		rb.AddForce(transform.up*10,ForceMode.Impulse);
		rb.AddForce(transform.forward*-10,ForceMode.Impulse);
		//rb.AddForce(Vector3.up * 10,ForceMode.Impulse);
	}

	void Move(){
		transform.LookAt(player.transform.position);
		rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
	}
}
