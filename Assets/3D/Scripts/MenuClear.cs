using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MenuClear : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().buildIndex != 0)
			gameObject.SetActive (false);
	
	}
}
