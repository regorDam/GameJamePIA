using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public float speed = 6f;
	public float FireRate = 0.0f;


	private FirstPersonController player;

    private GameObject owner;
    int damage;
    float lifeTime;

    // Use this for initialization


    public void Config(GameObject owner, int damage)
    {
        this.owner = owner;
        this.damage = damage;

        Config(owner, speed, damage, 30);



    }

    public void Config(GameObject owner, float speed, int damage, float lifeTime)
    {
        this.owner = owner;
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = lifeTime;


       Destroy(gameObject, lifeTime);
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 8)
        {
            Destroy(gameObject, 4);
        }
		if(owner.Equals(col.gameObject)) return;

		var hit = col.gameObject;
		var health = hit.GetComponent<Health>();

		if (health != null)
		{
			health.TakeDamage(damage);
			if (health.currentHealth <= 0 && player != null)
				player.AddScore(10);
		}
    }


}
