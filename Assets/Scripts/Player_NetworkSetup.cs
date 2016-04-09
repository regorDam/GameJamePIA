using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour 
{
	[SerializeField] Camera FPScam;
	[SerializeField]AudioListener audioListener;


	private GameObject SceneCam;
	// Use this for initialization
	void Start () 
	{
		if(isLocalPlayer)
		{
			SceneCam = GameObject.Find ("Scene Camera");
			SceneCam.SetActive (false);

			FPScam.enabled = true;
			GetComponent<FisrtPersonConroller> ().enabled = true;
			audioListener.enabled = true;
	
		}
	}

}
