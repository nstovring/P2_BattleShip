using UnityEngine;
using System.Collections;

public class Enemy : Entity {

	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void move(){
	}
	
	void attack(){
		
	}
	
	public void RecieveDamage (int damage){

		health -= damage;

		transform.GetComponent<followTarget>().Bounce();
		AudioClip clip =GetComponent<AudioSource>().clip;
		
		GetComponent<AudioSource>().PlayOneShot(clip);
		if (health <= 0) {
			Die();
		}

	}
	
	public void Die (){
		Destroy (this.gameObject); 
	}


}
