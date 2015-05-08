using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour, isDamagable {
	
	public int speed;
	public int health;
	public int damage;
	public int range;
	
	
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

	public void RecieveDamage (int damage){}
	
	public void Die (){}
}
