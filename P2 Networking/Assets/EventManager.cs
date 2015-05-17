using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	int destroyedShips;

	public delegate void Destroyed();
	public static event Destroyed onDestroyed;
	// Use this for initialization
	void Start () {
	
	}

	
	// Update is called once per frame
	void Update () {
		if(onDestroyed != null){
			destroyedShips ++;
		}
	}
}
