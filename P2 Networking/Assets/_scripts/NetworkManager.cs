using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "UniqueGameName";
	private const string gameName = "P2 Networking";

	public GameObject playerPrefab;
	public GameObject target;

	private HostData[] hostList;

	private void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	// Use this for initialization
	void OnServerInitialized()
	{
		SpawnPlayer();
		Debug.Log("Server Initializied");
	}
	void OnConnectedToServer()
	{
		SpawnPlayer();
		Debug.Log("Server Joined");
	}
	private void SpawnPlayer()
	{
		Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
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
		if(Network.isClient || Network.isServer){
			if(GUI.Button(new Rect(100, 100 + (110), 300, 100), "Change Color")){
				transform.GetComponent<NetworkView>().RPC("ChangeColor", RPCMode.All);
				//.transform.GetComponent<Renderer>().material.color = Color.green;
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

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	void ChangeColor(){
		target.transform.GetComponent<Renderer>().material.color = Color.green;
	}
}
