using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "UniqueGameName";
	private const string gameName = "P2 Networking";

	public createLayout[] Layouts = new createLayout[2];
	public GameObject playerPrefab;
	public GameObject target;
	public static int playerCount = Network.connections.Length;


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
		Debug.Log("Server Initializied");
	}
	void OnConnectedToServer()
	{
		//SpawnPlayer();
		Debug.Log("Server Joined");
		Debug.Log("Player ID is " + Network.player.ToString());
		playerCount = int.Parse(Network.player.ToString());

		for(int i = 0; i < Layouts.Length; i++){
			Layouts[i].CreateLayout();
		}
		Camera.main.transform.GetComponent<CameraScript>().AssignClientCamera(playerCount);

	}
	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player " + playerCount++ + " connected from " + player.ipAddress + ":" + player.port);
	}
	private void SpawnPlayer()
	{
		if(Network.isClient){
		Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
		}
		if(Network.isServer){
			Network.Instantiate(playerPrefab, new Vector3(5f, 5f, 0f), Quaternion.identity, 0);
		}
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
