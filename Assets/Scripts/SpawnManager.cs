using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour 
{
	
	public GameObject playerPref;
	public GameObject enemyPref;
	public Transform spawn1, spawn2;
	
	public int numPlayers = 2;
	
	void Start () 
	{
		for (int i = 0; i < numPlayers; ++i)
		{
			Spawn(i);
		}
	}
	
	void Spawn(int playerId)
	{
		GameObject player = (GameObject)Instantiate (playerPref, spawn1.position, Quaternion.identity);
		player.GetComponent<FirstPersonController>().playerId = playerId; 
		GameObject cam = player.transform.FindChild("Main Camera").gameObject; 
        cam.GetComponent<Camera>().rect = 
            new Rect(0.0f,  1.0f / numPlayers * playerId, 1.0f, 1.0f / numPlayers);
	}
}