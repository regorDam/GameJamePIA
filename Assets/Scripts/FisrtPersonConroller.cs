﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FisrtPersonConroller : MonoBehaviour
{
	//Player Settings
    public float mouseSensitivityX = 250f;
    public float mouseSensitivityY = 250f;
    public float walkSpeed = 8f;
    public float runSpeed = 14f;
    public float jumpForce = 220;
    public bool godMode;
    public float godForceMax = 5;
    float godForceMin = 1;
    float godForce;
	float runSpeedPower;
	float jumpForcePower;
    public LayerMask groundedMask;

	//Connections
	private WeaponController weaponController;
	Rigidbody rgb;

	//[SyncVar]
    public Transform cameraT;
    float verticalLookRotation;

	//Helpers
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
	bool grounded;
    bool dead;
	bool m_focus =true ;
	bool runPower = false;
	bool jumpPower = false;
	public Transform bulletSpawn;


	public float coldownPowerUp;

    void Start ()
    {
        //cameraT = Camera.main.transform;
     
	}


	void Awake()
	{
		weaponController = GameObject.Find ("Main Camera/Weapon").GetComponent<WeaponController> ();
		transform.SetParent (GameObject.Find ("World").transform);
		rgb = GetComponent<Rigidbody>();
	
	}
	void Update ()
    {
		
		if (!m_focus)
			return;
		
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        cameraT.localEulerAngles = Vector3.left * verticalLookRotation;

        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 targetMoveAmount;

        if (Input.GetKeyDown(KeyCode.P))
            godMode = !godMode;

        if (godMode)
            godForce = godForceMax;
        else
            godForce = godForceMin;

		coldownPowerUp -= Time.deltaTime;

		if (runPower && coldownPowerUp > 0)
			runSpeedPower = 5;
		else 
		{
			runPower = false;
			runSpeedPower = 1;
		}
			
		if (jumpPower && coldownPowerUp > 0)
			jumpForcePower = 5;
		else 
		{
			jumpPower = false;
			jumpForcePower = 1;
		}


        if ((Input.GetKey("left shift") || Input.GetKey("right shift")))
        {
            targetMoveAmount = moveDir * (runSpeed * godForce * runSpeedPower);

        }
        else
        {
            targetMoveAmount = moveDir * (walkSpeed + godForce);
        }
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        if(Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
				rgb.AddForce(transform.up * (jumpForce * godForce * jumpForcePower));
            }

        }
        
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

       if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
        {
            grounded = true;
        }
        else
        {
            //grounded = false;
        }


		if(Input.GetButtonDown("Fire1") && CheckFire())
		{
			Fire();
		}


    }

	void OnCollisionEnter(Collision col)
	{
		if(col.transform.tag.Equals("PowerUp"))
		{
			switch (col.transform.GetComponent<PowerUp> ().type) 
			{
			case 1:
				runPower = true;
				coldownPowerUp = 13;
				break;
			case 2:
				jumpPower = true;
				coldownPowerUp = 13;
				break;
			default:
				break;
			}
			Destroy (col.gameObject);
		} 

	}

    void OnCollisionStay(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.layer == 8)
        {
            grounded = true;
        }
    }
    void OnCollisionExit()
    {
        grounded = false;
    }

        void FixedUpdate()
    {
        rgb.MovePosition(rgb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

	void OnApplicationFocus(bool value){
		m_focus = value;

	}

	bool CheckFire()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{ // UI elements getting the hit/hover & is dead
			return false;
		}

		return true;
	}


	public void Fire()
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


}
