using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FirstPersonController : MonoBehaviour
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
	public float score = 0;
	public int playerId = 0;
	private const float maxCooldownAttack = 1f;
	private const float maxCooldownDefense = 2.5f;
	public float cooldownAttack = maxCooldownAttack;
	public float cooldownDefense = maxCooldownDefense;
	private bool isAttacking = false;
	private bool isDefending = false;
	public bool isBlocking = false;

	//Connections
	private WeaponController weaponController;
	Rigidbody rgb;
	public KnightHashIDs knightHash;
	public Animator anim;
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
	bool haloShield = false;
	bool enrage = false;
	public List<Transform> bulletSpawn = new List<Transform>();
	private bool hasPowerUp = false;


	public string nameknight = "knight_FBX_Walk";
	public string namedragon = "Dragon_FBX_WALK";

	public string nameObj;
	public Sprite spJump, spRun, spAura, spRage;


	public float coldownPowerUp;
	public class KnightHashIDs{
		public int die;
		public int attack;
		public int run;
		public int walk;

		public KnightHashIDs(Animator refAnim){
			die = Animator.StringToHash("Die");
			attack =Animator.StringToHash("Attack");
			run  =Animator.StringToHash("Run");
			walk =Animator.StringToHash("WalkSpeed"); 
		}
	}
	void Start ()
	{
		//cameraT = Camera.main.transform;
		transform.SetParent (GameObject.Find ("World").transform);

		if (playerId == 1) {
			nameObj = namedragon;
			transform.Find (nameObj).gameObject.SetActive (true);
			transform.Find (nameknight).gameObject.SetActive (false);

		} else {
			nameObj = nameknight;
			transform.Find (nameObj).gameObject.SetActive (true);
			transform.Find (namedragon).gameObject.SetActive (false);
		}
		anim = transform.Find(nameObj).GetComponent<Animator>();
	}


	void Awake()
	{



		knightHash = new KnightHashIDs(anim);
		weaponController = GameObject.Find ("Main Camera/Weapon").GetComponent<WeaponController> ();

		rgb = GetComponent<Rigidbody>();

	}
	void Update ()
	{

		if (!m_focus)
			return;

		Vector3 rotX = Vector3.up * Input.GetAxis("RightJoystickX" + playerId) *  Time.deltaTime * mouseSensitivityX;
		if(rotX.magnitude > 0.1f) transform.Rotate(rotX);

		float rotY = Input.GetAxis("RightJoystickY" + playerId) * Time.deltaTime *  mouseSensitivityY;
		if (Mathf.Abs(rotY) > 0.1f)
		{
			verticalLookRotation += rotY;
			verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
		}

		cameraT.localEulerAngles = Vector3.left * verticalLookRotation;

		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal" + playerId), 0, 
			Input.GetAxisRaw("Vertical" + playerId)).normalized;
		Vector3 targetMoveAmount;

		if (Input.GetKeyDown(KeyCode.P))
			godMode = !godMode;

		if (godMode)
			godForce = godForceMax;
		else
			godForce = godForceMin;

		coldownPowerUp -= Time.deltaTime;


		////// CHECK powerUPS
		bool check = true;
		if (runPower && coldownPowerUp > 0) {
			runSpeedPower = 5;
			check = false;
		}
		else 
		{
			runPower = false;
			runSpeedPower = 1;
		}

		if (jumpPower && coldownPowerUp > 0) {
			jumpForcePower = 5;
			check = false;
		}
		else 
		{
			jumpPower = false;
			jumpForcePower = 1;
		}

		if (haloShield && coldownPowerUp < 0) {
			haloShield = false;
		} else {
			check = false;
		}

		if (enrage && coldownPowerUp < 0) {
			enrage = false;
		} else {
			check = false;
		}


		if(check)
			GetComponent<HUDManager>().imgPowerUp.GetComponent<Image> ().sprite = Resources.Load("Sprites/PoUp_Icon_Disabled", typeof(Sprite)) as Sprite;


		if ((Input.GetButton("Run" + playerId) || Input.GetKey("right shift")))
		{
			targetMoveAmount = moveDir * (runSpeed * godForce * runSpeedPower);
			//RUN CALLANIMATOR
			anim.SetBool(knightHash.run, true);
		}
		else
		{
			anim.SetBool(knightHash.run, false);
			targetMoveAmount = moveDir * (walkSpeed + godForce);
		}
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

		if(Input.GetButtonDown("Jump" + playerId))
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

		// attacking

		/*
		cooldownAttack -= Time.deltaTime;
		if (cooldownAttack < 0) 
		{
			cooldownAttack = maxCooldownAttack;
			isAttacking = false;
		}
		*/

		if(Input.GetButtonDown("Fire" + playerId))
		{
			if (playerId == 1) {
				FireDragon ();
			} else {
				Fire ();
			}
			anim.SetTrigger (knightHash.attack);
			//FIRE CALLANIMATOR
		}

		// defending
		if (isDefending) 
		{
			cooldownDefense -= Time.deltaTime;
			if (cooldownDefense < 0) 
			{
				cooldownDefense = maxCooldownDefense;
				isDefending = false;
			}
		}

		if (Input.GetButtonDown ("Block" + playerId)) {
			isDefending = true;
			isBlocking = true;

		} else 
		{
			isBlocking = false;
		}


		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		foreach(GameObject player in players)
		{

			if (player.GetComponent<Health> ().lifes <= 0) 
			{
				SceneManager.LoadScene ("Menu");
			}
		}
	}


	void OnCollisionEnter(Collision col)
	{
		
		if (col.transform.tag.Equals("PowerUp"))
		{
			hasPowerUp = true;
			switch (col.transform.GetComponent<PowerUp> ().type) {
			case 1:
				runPower = true;
				coldownPowerUp = 13;
				GetComponent<HUDManager>().imgPowerUp.GetComponent<Image> ().sprite = Resources.Load ("Sprites/PoUp_Icon_Run", typeof(Sprite)) as Sprite;
				break;
			case 2:
				jumpPower = true;
				coldownPowerUp = 13;
				GetComponent<HUDManager>().imgPowerUp.GetComponent<Image> ().sprite = Resources.Load ("Sprites/PoUp_Icon_Jump", typeof(Sprite)) as Sprite;
				break;
			case 3:
				haloShield = true;
				coldownPowerUp = 5;
				GetComponent<HUDManager>().imgPowerUp.GetComponent<Image> ().sprite = Resources.Load ("Sprites/PoUp_Icon_Escudo", typeof(Sprite)) as Sprite;
				break;
			case 4:
				enrage = true;
				coldownPowerUp = 6;
				GetComponent<HUDManager>().imgPowerUp.GetComponent<Image> ().sprite = Resources.Load ("Sprites/PoUp_Icon_Rage", typeof(Sprite)) as Sprite;
				break;
			default:
				
				break;
			}
			Destroy (col.gameObject);



		}
			

		if (col.gameObject.layer == 10 && playerId == 1 || col.gameObject.layer == 11 && playerId == 2) 
		{
			col.rigidbody.AddForce ((transform.position - col.transform.position) * Time.deltaTime);
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
		//WALK CALL ANIMATOR rgb.velocity
		anim.SetFloat(knightHash.walk, moveAmount.magnitude);
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

		if (cooldownAttack > 0) 
		{
			//return false;
		}

		return true;
	}


	public void Fire()
	{
		GameObject bullet;
		if (enrage) {
			for (int x = 0; x < 3; x++) 
			{
				bullet = (GameObject)Instantiate (Resources.Load ("Prefabs/Bullet2", typeof(GameObject)), bulletSpawn [x].position, bulletSpawn [x].rotation);
				bullet.GetComponent<Bullet> ().Config (gameObject, 100);
				//bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bullet.GetComponent<Bullet>().speed;
				bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.forward * bullet.GetComponent<Bullet> ().speed);

			}
		} else 
		{
			bullet = (GameObject)Instantiate (Resources.Load ("Prefabs/Bullet", typeof(GameObject)), bulletSpawn [0].position, bulletSpawn [0].rotation);
			bullet.GetComponent<Bullet> ().Config (gameObject, 40);
			//bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bullet.GetComponent<Bullet>().speed;
			bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.forward * bullet.GetComponent<Bullet> ().speed);
		}


		if(!GetComponentInChildren<AudioSource>().isPlaying)
		{
			GetComponentInChildren<AudioSource>().Play();
		}

	}

	public void FireDragon()
	{
		GameObject bullet;
		if (enrage) {
			for (int x = 0; x < 3; x++) 
			{
				bullet = (GameObject)Instantiate (Resources.Load ("Prefabs/fireball", typeof(GameObject)), bulletSpawn [x].position, bulletSpawn [x].rotation);
				bullet.GetComponent<Bullet> ().Config (gameObject, 100);
				//bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bullet.GetComponent<Bullet>().speed;
				bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.forward * bullet.GetComponent<Bullet> ().speed);

			}
		} else 
		{
			bullet = (GameObject)Instantiate (Resources.Load ("Prefabs/fireball", typeof(GameObject)), bulletSpawn [0].position, bulletSpawn [0].rotation);
			bullet.GetComponent<Bullet> ().Config (gameObject, 40);
			//bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bullet.GetComponent<Bullet>().speed;
			bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.forward * bullet.GetComponent<Bullet> ().speed);
		}


		if(!GetComponentInChildren<AudioSource>().isPlaying)
		{
			GetComponentInChildren<AudioSource>().Play();
		}

	}



	public void AddScore(int score)
	{
		this.score += score;
	}


}
