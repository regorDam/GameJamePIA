using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public const int maxHealth = 200;
	public int lifes = 3;

	int maxLifes = 3;
   
    public int currentHealth = maxHealth;

    public RectTransform healthBar;
    public bool destroyOnDeth;

	private GameObject[] respawns;

    void Start()
    {
		respawns = GameObject.FindGameObjectsWithTag("Respawn");
    }


	void Update()
	{
		OnChangeHealth ();
	}

    public void TakeDamage(int amount)
    {
		if (transform.GetComponent<FirstPersonController> ().isBlocking)
		{
			return;
		}
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
			transform.GetComponent<FirstPersonController> ().anim.SetTrigger (transform.GetComponent<FirstPersonController> ().knightHash.die);	
            if (destroyOnDeth)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
				lifes--;
				if (lifes > 0) {
				 	Respawn();
				}
            }
        }   
    }

    void OnChangeHealth()
    {
		healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    void Respawn()
    {
            Vector3 spawnPoint = Vector3.zero;

			if(respawns != null && respawns.Length > 0)
            {
			spawnPoint = respawns[Random.Range(0, respawns.Length - 1)].transform.position;
			if(lifes == 1)
				spawnPoint = respawns[respawns.Length].transform.position;
            }

            transform.position = spawnPoint;
    }



	public int GetLifes(){
		return lifes;
	}

	public int GetMaxLifes(){
		return maxLifes;
	}
}
