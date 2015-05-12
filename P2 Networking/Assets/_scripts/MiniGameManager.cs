using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class MiniGameManager : MonoBehaviour {

	public static int[] TeamScore = new int[2];
	protected NetworkView nView;
	//StateMachine NetworkView
	public int MiniGameScoreMin = 5;
	private NetworkView sNView;
	bool newTask = false;
	CanvasController canvasController;

	string[] buttonNames = {"Steering-rod", "Drain holes", "Air-flask", "Immersion-chamber", "Steering Engine", "Bevel-gear box", "Stop-valves", "Engine bed plate", "Primer case", "Propellers", "Guide stud", "Strengthening band"};
	string[] buttonCommands = {"Adjust the ", "Flush out the ", "Refill the ", "Unimmerse the ", "Reignite the ", "Fetch the ", "Turn the ", "Clear out the", "Re-arrange the ", "Engage the ", "Pull up the ", "Fix the"};

	GameObject[] miniGameButtons = new GameObject[6];
	public GameObject taskDisplayer;
	public StateMachine stateMachine;
	public NetworkManager networkManager;
	Text[] scoreTexts = new Text[1];

	//used for storing random numbers that are used when assigning button names.
	List<int> randomList = new List<int>();



	// Use this for initialization
	void Start () {
		//Get the canvas controller class
		canvasController = GameObject.Find("Canvas").GetComponent<CanvasController>();
		//Store the array of minigame buttons
		miniGameButtons = canvasController.miniGameButtons;
		//Get the gameobjects NetworkView
		nView = GetComponent<NetworkView>();
		sNView = stateMachine.GetComponent<NetworkView>();
		scoreTexts = canvasController.scoreText;
		//Assign names to each of the buttons 

		//For loop for assignin button names
		for(int i = 0; i < miniGameButtons.Length; i++){

			//find a random number, this is used to fetch a position from the buttonNames array
			int randomButtonName = Random.Range(0,buttonNames.Length);
			//as long as this random number is found in randomList, generate a new random number and repeat process
			while (randomList.Contains(randomButtonName)){
				randomButtonName = Random.Range(0,buttonNames.Length);
			}
	
			//if the random number is not found in randomList, add it and use it to fetch a name from the buttonNames array
			if (!randomList.Contains(randomButtonName)){
				randomList.Add(randomButtonName);
				miniGameButtons[i].GetComponent<MiniGameButton>().name = buttonNames[randomButtonName];
			}

	
		}
	}

	// Update is called once per frame
	void Update () {
		if(TeamScore[0] >=MiniGameScoreMin){
			sNView.RPC("ChangeState",RPCMode.AllBuffered, 2);
			sNView.RPC("SetTeamTurn",RPCMode.AllBuffered, 1);
			TeamScore[0] = 0;
			noTasks1 = false;
			noTasks2 = false;
		}else if(TeamScore[1] >= MiniGameScoreMin){
			sNView.RPC("ChangeState",RPCMode.AllBuffered, 2);
			sNView.RPC("SetTeamTurn",RPCMode.AllBuffered, 2);
			TeamScore[1] = 0;
			//noTasks1 = true;
			//noTasks2 = true;
		}
		//At the start of a game assign a task for each player
		InitializeMiniGames();
	}
	//At the beggining of each minigame round noTasks 1 & 2 are true
	bool noTasks1 = true;
	bool noTasks2 = true;
	void InitializeMiniGames(){
		if(noTasks1 && Network.connections.Length >= 2){
			nView.RPC("AssignNewTask",Network.connections[0]);
			nView.RPC("AssignNewTask",Network.connections[1]);
			noTasks1 = false;
		}else if(noTasks1 && Network.connections.Length >= 4){
			nView.RPC("AssignNewTask",Network.connections[2]);
			nView.RPC("AssignNewTask",Network.connections[3]);
			noTasks2 = false;
		}
	}

	//Sets the text in the taskDisplayer
	[RPC]
	public void SetTaskDisplayerText(string task){
		Debug.Log("SetTaskDisplayer " + task);
		taskDisplayer.GetComponent<Text>().text = "Press the "+ task + " button";
	}
	//Assign tasks to a specific player dependant on which one called the method
	[RPC]
	private void InquireNewTask(NetworkPlayer inquirer){
		if(inquirer == Network.connections[0]){
			nView.RPC("AssignNewTask",Network.connections[1]);
		}
		else if(inquirer == Network.connections[1]){
			nView.RPC("AssignNewTask",Network.connections[0]);
		}
		else if(inquirer == Network.connections[2]){
			nView.RPC("AssignNewTask",Network.connections[3]);
		}
		else if(inquirer == Network.connections[3]){
			nView.RPC("AssignNewTask",Network.connections[2]);
		}
	}
	//The InquireSetTaskDisplayerText method changes the task displayed for the teammate of the player who Inquired 
	[RPC]
	void InquireSetTaskDisplayerText(string task, NetworkMessageInfo info){
		if(info.sender == Network.connections[0]){
			nView.RPC("SetTaskDisplayerText",Network.connections[1], task);
			nView.RPC("UpdateScore",RPCMode.AllBuffered,0,1);
		}
		else if(info.sender == Network.connections[1]){
			nView.RPC("SetTaskDisplayerText",Network.connections[0], task);
			nView.RPC("UpdateScore",RPCMode.AllBuffered,0,1);
		}
		else if(info.sender == Network.connections[2]){
			nView.RPC("SetTaskDisplayerText",Network.connections[3], task);
			nView.RPC("UpdateScore",RPCMode.AllBuffered,1,1);
		}
		else if(info.sender == Network.connections[3]){
			nView.RPC("SetTaskDisplayerText",Network.connections[2], task);
			nView.RPC("UpdateScore",RPCMode.AllBuffered,1,1);
		}
	}

	int previousTask;
	[RPC]
	void AssignNewTask(NetworkMessageInfo info){
		//Find a random integer to determine next task
		int rng = Random.Range(0,miniGameButtons.Length);
		//Ensure that this integer is not equal to the previously found integer, else find a new int
		int rngTask = rng != previousTask ? rng: Random.Range(0,miniGameButtons.Length);
		//If any button is Active do not assign a new task
		for(int i = 0; i < miniGameButtons.Length; i++){
			if(miniGameButtons[i].GetComponent<MiniGameButton>().isActive){
				return;
			}
		}
		//Set a random button to active
		miniGameButtons[rngTask].GetComponent<MiniGameButton>().isActive = true;

		//Get the name of the randomly selected button + a random instruction from the buttonCommands and assign them to the new task

		int rng2 = Random.Range(0,buttonCommands.Length);
		string task = buttonCommands[rng2] + miniGameButtons[rngTask].GetComponent<MiniGameButton> ().name;

		//Request server to change the taskDisplayers text
		nView.RPC("InquireSetTaskDisplayerText",RPCMode.Server, task);
		//nView.RPC("UpdateScore",RPCMode.Server, Network.player);
		previousTask = rngTask;
		newTask = true;
	}

	[RPC]
	void UpdateScore(int team, int val){
		TeamScore[team] += val;
		Debug.Log("Current score is: " + TeamScore[0] + "for team 1 and: " + TeamScore[1] + "for team 2");
		scoreTexts [team].GetComponent<Text> ().text = "Your teams score is: " + TeamScore;
	}
}
