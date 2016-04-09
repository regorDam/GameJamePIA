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
		player.GetComponent<FirstPersonConroller>().playerId = playerId; 
		GameObject cam = player.transform.FindChild("Main Camera").gameObject;
        GameObject hudCam = cam.transform.FindChild("HUDCamera").gameObject;
        cam.GetComponent<Camera>().rect = 
            new Rect(0.0f,  1.0f / numPlayers * playerId, 1.0f, 1.0f / numPlayers);

        Rect camRect = cam.GetComponent<Camera>().rect;
        hudCam.GetComponent<Camera>().rect = camRect; // viewport rect

        //ortho rect :)
        GameObject hudCanvas = cam.transform.FindChild("HUD Canvas").gameObject;
        Rect hudCanvasRect = hudCanvas.GetComponent<RectTransform>().rect;
        hudCam.GetComponent<Camera>().aspect = hudCanvasRect.width / hudCanvasRect.height;
        hudCam.GetComponent<Camera>().orthographicSize = hudCanvasRect.height;

        /*
        hudCam.GetComponent<Camera>().aspect = camRect.width / camRect.height;
        hudCam.GetComponent<Camera>().orthographicSize = camRect.height;
    */
    }
}