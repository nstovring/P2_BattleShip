using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	float spawnRate = 10f;
	float spawnTimer;
	public Sprite [] enemies;
	public GameObject playerPrefab;
	public GameObject enemyPrefab;
	
	Vector3 position;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(Random.Range(-25.0F, -15.0F),0,0);
		spawnRate += Time.deltaTime * 1;
		if(spawnRate >= 0){
			SpawnEnemy();
			spawnRate = 0;
		}
	}
	
	void SpawnEnemy(){
		GameObject clone =Instantiate(enemyPrefab,transform.position,Quaternion.identity) as GameObject;
		clone.transform.GetComponent<SpriteRenderer> ().sprite = enemies [Random.Range (0, enemies.Length)];
		
	}
}