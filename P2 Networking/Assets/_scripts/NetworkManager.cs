using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "UniqueGameName";
	private const string gameName = "P2 Networking";

	public GameObject teamText; 
	public int thisPlayer;
	public createLayout[] Layouts = new createLayout[2];
	public ShipPlacement shipPlacement;
	public GameObject playerPrefab;
	public GameObject target;
	public static int playerCount = Network.connections.Length;
	//int state
	public static NetworkPlayer[] players = new NetworkPlayer[4];
	public static int playerID;
	private HostData[] hostList;

	private void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	// Use this for initialization
	void OnServerInitialized()
	{
		Camera.main.transform.GetComponent<CameraScript>().AssignServerCamera();
		for(int i = 0; i < Layouts.Length; i++){
			Layouts[i].CreateLayout();
		}
		teamText.GetComponentInChildren<Text>().text = "Board View";
		teamText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-440,0);

		Debug.Log("Server Initializied");
	}
	void OnConnectedToServer()
	{
		if(int.Parse(Network.player.ToString()) == 1 || int.Parse(Network.player.ToString()) == 2){
			teamText.GetComponentInChildren<Text>().text = "Team 1";
		}else{
			teamText.GetComponentInChildren<Text>().text = "Team 2";
		}
		teamText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-440,-300);
		Debug.Log("Server Joined " + "Player ID is " + Network.player.ToString());
		playerCount = int.Parse(Network.player.ToString());
		thisPlayer = playerCount;
		for(int i = 0; i < Layouts.Length; i++){
			Layouts[i].CreateLayout();
		}
		shipPlacement.SpawnGhostShips();
		Camera.main.transform.GetComponent<CameraScript>().AssignClientCamera(playerCount);

	}
	void OnPlayerConnected(NetworkPlayer player) {
		playerID = int.Parse(player.ToString());
		Debug.Log("Player " + playerID + " connected from " + player.ipAddress + ":" + player.port);
		Debug.Log("Amount of players connected is " + Network.connections.Length);

		GetComponent<NetworkView>().RPC("setPlayers",RPCMode.AllBuffered, player, playerID);

	}
	[RPC]
	void setPlayers(NetworkPlayer player, int _playerID){
		players[_playerID -1] = player;
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
}
