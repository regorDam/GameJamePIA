using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class HUDManager : MonoBehaviour 
{

	private GameObject[] imgLifes;
	private Health health;

	public GameObject imgPowerUp;


	// Use this for initialization
	void Start () {
	
		imgLifes = GameObject.FindGameObjectsWithTag ("life");

		health = transform.GetComponent<Health> ();

		imgPowerUp = transform.Find ("Main Camera/HUD Canvas/Panel/PowerUpBag").gameObject;

	}



	// Update is called once per frame
	void Update () {

		//// UPDATE LIFES ////
		int lifes = health.GetLifes();
		int maxLifes = health.GetMaxLifes();
		for (int x = maxLifes; x > 0; x--)
		{
			if(lifes >= x)
				transform.Find("Main Camera/HUD Canvas/Panel/Lifes/Life " + x).gameObject.GetComponent<Image>().enabled = true;
			else
				transform.Find("Main Camera/HUD Canvas/Panel/Lifes/Life " + x).gameObject.GetComponent<Image>().enabled = false;
		}

	}
}
