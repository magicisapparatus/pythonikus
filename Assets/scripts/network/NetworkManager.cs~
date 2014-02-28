using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour 
{
	//data and variables and stuff
	int ServerCount;
	string registeredName="pythonikus_test";
	public float refreshRequestLength=3.0f;
	HostData[] hostData;


	//this is for making unities master server to work with, I guess
	//then Ill make something happen with it
	private void StartServer()
	{
		Network.InitializeServer(16,25566,false);
		MasterServer.RegisterHost(registeredName,"Networking test","yes, its for pythonikus");
	}
	
	void OnServerInitialized()
	{
		Debug.Log("server started");
	}

	void OnMasterServerEvent(MasterServerEvent masterServerEvent)
	{
		if(masterServerEvent==MasterServerEvent.RegistrationSucceeded)
			Debug.Log("registration success dawg!");
	}


	//this is for the refreshing
	public IEnumerator RefreshHostList()
	{
		Debug.Log("refreshing bra...");
		MasterServer.RequestHostList(registeredName);
		float timeEnd=Time.time+refreshRequestLength;;

		while(Time.time<timeEnd)
		{
			hostData=MasterServer.PollHostList();
			yield return new WaitForEndOfFrame();
		}

		if(hostData==null||hostData.Length==0)
		{
			Debug.Log ("No active servers bro");
		}
		else
			Debug.Log(hostData.Length+" servers are here bra");
	}


	//here is the code for the buttons and what to do
	public void OnGUI()
	{
		//get rid of the buttons if your connected
		if(Network.isClient||Network.isServer)
		return;


		if(GUI.Button(new Rect(25f,25f,150f,30f),"start new server"))
		   {
			//server function goes here
			StartServer();
		}

		if(GUI.Button(new Rect(25f,65f,150f,30f),"refresh"))
		{



			//y'know, for a refresh button
			StartCoroutine("RefreshHostList");
			}

		for(ServerCount=0; ServerCount<hostData.Length; ServerCount++)
		{
			if
(GUI.Button(new Rect(Screen.width/2,65f+(30f*ServerCount),300f,30f),hostData[ServerCount].gameName))
			{
				Network.Connect(hostData[ServerCount]);
			}
		}
	}
}