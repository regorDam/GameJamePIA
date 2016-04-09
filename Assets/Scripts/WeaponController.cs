using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


public class WeaponController : NetworkBehaviour
{
    public Transform bulletSpawn;




	[Command]
    public void CmdFire()
    {
        GameObject bullet;
        bullet = (GameObject)Instantiate(Resources.Load("Prefabs/Bullet", typeof(GameObject)), bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Bullet>().Config(gameObject, 2);
        //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bullet.GetComponent<Bullet>().speed;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bullet.GetComponent<Bullet>().speed);
        if(!GetComponentInChildren<AudioSource>().isPlaying)
        {
            GetComponentInChildren<AudioSource>().Play();
        }

    }

    bool CheckFire()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        { // UI elements getting the hit/hover & is dead
            return false;
        }

        return true;
    }

}
