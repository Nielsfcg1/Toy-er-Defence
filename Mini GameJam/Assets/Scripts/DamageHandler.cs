using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour {

	public int health = 30;
	public GameObject blood;
	public GameObject[] bloodSplatter;
	public Gibs[] gibs;
	public AudioClip[] audioClips;
	public AudioClip[] moverDeathSounds;
	AudioSource audioSource;

	bool isDead;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			Death();
		}
	}

	void Death () {
		if (!isDead)
		{
			GetComponent<Mover>().OnDeath();

			GetComponent<Mover>().spriteRenderer.enabled = false;
			isDead = true;
			Object.Instantiate(blood, transform.position, Quaternion.Euler(90, 0, 0));
			//Debug.Log("Mover died");
			audioSource.PlayOneShot(moverDeathSounds[Random.Range(0, moverDeathSounds.Length)]);
			//print("playsound");
			for (int i = Random.Range(0, 3); i >= 0; i--)
			{
				//Debug.Log("Spawning gib " + i);

				Gibs gib = Object.Instantiate(gibs[Random.Range(0, 3)], transform.position, Quaternion.Euler(90, 0, 0));
                gib.Initialize(transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
            }

			StartCoroutine(WaitForDeath());
		}
	}

	private void OnTriggerEnter(Collider col) {
		if (col.tag == "Bullet")
		{
			health -= col.GetComponent<Bullet>().damageOutput;
			Object.Instantiate(bloodSplatter[Random.Range(0,bloodSplatter.Length)], transform.position + col.GetComponent<Bullet>().destination.normalized, col.transform.rotation);

			Destroy(col.gameObject);
			
			if (this.tag == "Father")
			{
				audioSource.PlayOneShot(audioClips[0]);
				this.GetComponent<Parent>().BecomeAngry();
			}
			else if (this.tag == "Mother")
			{
				audioSource.PlayOneShot(audioClips[1]);
				this.GetComponent<Parent>().BecomeAngry();
			}
			else if (this.tag == "Mover")
			{
				if(health > 0)
					audioSource.PlayOneShot(audioClips[2]);
			}
		}
	}

	IEnumerator WaitForDeath()
	{
		yield return new WaitForSeconds(1);

		Destroy(this.gameObject);
	}
}
