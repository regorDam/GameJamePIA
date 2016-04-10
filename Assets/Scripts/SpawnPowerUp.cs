using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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


	GameObject princesGO1, princesGO2;
	// Use this for initialization
	void Start () 
	{
		princesGO1 = (GameObject) Instantiate (princes1, spawnPositionPrinces.position, transform.rotation);
		princesGO2 = (GameObject) Instantiate (princes2, spawnPositionPrinces.position, transform.rotation);
		princesGO1.transform.SetParent (transform);
		princesGO1.SetActive (false);
		princesGO2.transform.SetParent (transform);
		princesGO2.SetActive (false);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		time -= Time.deltaTime;
		if (time < 0) 
		{
		
			time = maxTimeSpawn;
			Spawn ();
			princesGO1.SetActive (true);
		}

	
	}


	void OnTriggerEnter(Collider other)
	{

		if (SceneManager.GetActiveScene ().name == "Menu")
			return;

		if (other.transform.GetComponent<FirstPersonController> () == null)
			return;



		if (other.transform.GetComponent<FirstPersonController> ().playerId == 1) {
			if (powerUp == null)
				princesGO1.SetActive (true);
				princesGO2.SetActive (false);
		} else {
			if (powerUp == null) {
				princesGO1.SetActive (false);
				princesGO2.SetActive (true);
			}
		}
	}

	void Spawn()
	{
		powerUp = (GameObject) Instantiate(PowerUps[Random.Range(0, PowerUps.Count -1)], spawnPosition.position, transform.rotation);
		powerUp.transform.SetParent (transform);
		Destroy (powerUp, 5);

		
	}
}
