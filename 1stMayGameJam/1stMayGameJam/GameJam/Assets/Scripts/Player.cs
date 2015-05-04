using UnityEngine;
using System.Collections;

public class Player : Entity {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"))* speed;
	
	}



	void OnCollisionEnter(Collision other ){
		
		
		//Enemy hits
		if (other.gameObject.tag == "Enemy"){
			
			RecieveDamage(2);
			
		}
		
	}
	
	
	void RecieveDamage (int damage){
		health -= damage;
		
		if (health <= 0) {
			Die();
		}
	}
	
	void Die (){
		
		Application.LoadLevel(0); //restarts game
		
	}
}
