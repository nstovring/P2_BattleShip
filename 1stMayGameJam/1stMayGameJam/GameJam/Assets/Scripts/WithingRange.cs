using UnityEngine;
using System.Collections;

public class WithingRange : MonoBehaviour {


//	int dmg = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().damage;

	int dmg = 5;



	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay (Collider other) {



		if (other.gameObject.tag == "Enemy" && Input.GetKeyUp(KeyCode.Space)){

			//Rigidbody enemeyBody = other.transform.GetComponent<Rigidbody>();
			AudioClip clip =GetComponent<AudioSource>().clip;

			GetComponent<AudioSource>().PlayOneShot(clip);
			//enemeyBody.AddForce(Vector3.up * 10);
			//enemeyBody.velocity = enemeyBody.velocity*-10;
			
			other.transform.GetComponent<Enemy>().RecieveDamage(dmg);

	}
}

}//end class
