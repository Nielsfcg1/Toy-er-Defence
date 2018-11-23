using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChar : MonoBehaviour {

	public static PlayerChar Instance;

	public GameObject gun;
	public bool gunPossessed = true;
	public bool gunEquipped = false;
	public float gunFiringRate = 0.5f;   // in sec
	public float gunTimer = 0.25f;
	public float bulletSpeed = 10f;
	public GameObject bullet;
	public float movementSpeed = 1f;
	public float bulletRange = 50f;
	public int bulletDamage = 10;
	RaycastHit hit;
	public GameObject bulletSpawn;
	public LayerMask layermask;
	public int HP;
	public float pickupDropRadius = 3f;
	public PickupNerfGun gunPickup;
	GameManager gm;

	public AudioSource audioSource;
	public AudioClip shootingSound;
	public AudioClip playerHit;
	public AudioClip pickupCandy;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		gun.SetActive(gunEquipped);
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		audioSource = GetComponent<AudioSource>();
	}

	
	void Update () {
		Weapon();
	}

	// equip, unequip, reload weapon and check if it is fired.
	void Weapon() {
		if (Input.GetMouseButtonDown(1) && gunPossessed) {
			gun.SetActive(!gun.activeInHierarchy);
			gunEquipped = !gunEquipped;

			//Added by patrick
			if (GetComponent<BuildBehaviour>().objectVisible)
			{
				GetComponent<BuildBehaviour>().prevGO.SetActive(false);
				GetComponent<BuildBehaviour>().objectVisible = false;
				GetComponent<BuildBehaviour>().prevGOUI.GetComponent<Image>().color = Color.white;
			}



		}
		if (gunEquipped && Input.GetMouseButton(0)) {
			if (gunTimer >= gunFiringRate) {
				FireWeapon();
				gunTimer = 0;
			}
			gunTimer += Time.deltaTime;
		}
		else if (gunTimer < gunFiringRate) {
			gunTimer += Time.deltaTime;
		}
	}

	void FireWeapon () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 1000f, layermask)) {

			Vector3 bulletAngle = hit.point - bulletSpawn.transform.position;
			bulletAngle = bulletAngle.normalized * bulletRange;
			bulletAngle = bulletSpawn.transform.position + bulletAngle;
			bulletAngle.y = 0;
			Bullet bul = Object.Instantiate(bullet, bulletSpawn.transform.position, gun.transform.rotation).GetComponent<Bullet>();
			bul.Initialize(bulletAngle, bulletSpeed, bulletDamage);

			audioSource.pitch = Random.Range(0.7f, 1.3f);
			audioSource.PlayOneShot(shootingSound);
		}
	}

	public void DropWeapon () {
		gunPossessed = false;
		gunEquipped = false;
		gun.SetActive(false);

		float ang = Random.value * 360;
		Vector3 pos;
		pos.x = transform.position.x + pickupDropRadius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.z = transform.position.z + pickupDropRadius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.y = transform.position.y;

		PickupNerfGun nerfPick = Object.Instantiate(gunPickup, transform.position, gun.transform.rotation);
        nerfPick.Initialize(pos);

	}

	//use to give damage to the player
	public void ApplyDamage()
	{

		if (gunPossessed) {
			DropWeapon();
		}

		HP -= 1;

		if (HP >= 0)
		{
			gm.ChangeHP(HP);
			if(HP == 0)
			{
                gm.gameOverText.text = "You have lost all your health...";
                gm.GameOver();
            }
		}

		audioSource.pitch = Random.Range(0.9f, 1.1f);
		audioSource.PlayOneShot(playerHit);
		//Push back player?  
	}
}
