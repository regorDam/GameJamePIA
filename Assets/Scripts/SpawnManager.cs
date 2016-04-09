using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour 
{

	public GameObject playerPref;
	public GameObject enemyPref;
	public Transform spawn1, spawn2;

	// Use this for initialization
	void Start () 
	{
		Spawn ();
	
	}
	
	void Spawn()
	{
		GameObject player = (GameObject)Instantiate (playerPref, spawn1.position, Quaternion.identity);

		
	}
}
