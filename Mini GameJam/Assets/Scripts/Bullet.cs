using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector3 destination;
    float speed;
    public int damageOutput;
    public LayerMask layerMask;


    public void Initialize (Vector3 dest, float sped, int dam) {
        destination = dest;
        speed = sped;
        damageOutput = dam;
        //transform.rotation = Quaternion.Euler(dest);
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        if (Physics.Raycast(transform.position, transform.position - destination, speed * Time.deltaTime, layerMask)) {
            Destroy(this.gameObject);
        }
        else if (transform.position == destination) {
            Destroy(this.gameObject);
        }
    }
}
