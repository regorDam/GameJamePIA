using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPowerUp : MonoBehaviour 
{


	public Transform spawnPositionPrinces;
	public Transform spawnPosition;

	public List<GameObject> PowerUps;

	public GameObject princes1;
	public GameObject princes2;

	private GameObject powerUp;
	private float maxTimeSpawn = 30;
	private float time;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		time -= Time.deltaTime;
		if (time < 0) 
		{
		
			time = maxTimeSpawn;
			Spawn ();
		}




		
	
	}


	void OnTriggerEnter(Collider other)
	{

		if (other.transform.GetComponent<FirstPersonController> ().playerId == 1) {
			if (powerUp == null)
				Instantiate (princes1, spawnPositionPrinces.position, transform.rotation);
		} else {
			if (powerUp == null)
				Instantiate (princes2, spawnPositionPrinces.position, transform.rotation);
		}
	}

	void Spawn()
	{
		powerUp = (GameObject) Instantiate(PowerUps[Random.Range(0, PowerUps.Count -1)], spawnPosition.position, transform.rotation);


		
	}
}
