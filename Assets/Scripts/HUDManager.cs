using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class HUDManager : MonoBehaviour 
{

	private GameObject[] imgLifes;
	private Health health;

	// Use this for initialization
	void Start () {
	
		imgLifes = GameObject.FindGameObjectsWithTag ("life");

		health = transform.GetComponent<Health> ();



	}
	
	// Update is called once per frame
	void Update () {
	
		for (int x = imgLifes.Length; x > 0; x--) 
		{
			if (x > health.lifes) 
			{
				imgLifes [x].SetActive (false);
			}
		}

	}
}
