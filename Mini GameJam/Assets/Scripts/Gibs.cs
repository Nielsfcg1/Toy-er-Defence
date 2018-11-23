using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gibs : MonoBehaviour {

    public float rotationSpeed = 4;
    float rotateTimer;
    float prevRotateTimer;

    public bool inPosition = false;
    public Vector3 goToLocation;
    public float movementSpeed = 1;
    public LayerMask layerMask;

    public int points;

    void Start()
    { 
        StartCoroutine(RotateCandy2());
	}
	
	// Update is called once per frame
	void Update () {
        CandyPlacement();
	}

    public void Initialize(Vector3 endPos) {
        goToLocation = endPos;
    }

    void CandyPlacement () {
        if (!inPosition) {
            if (!Physics.Raycast(transform.position, goToLocation - transform.position, Time.deltaTime * movementSpeed, layerMask)) {
                transform.position = Vector3.MoveTowards(transform.position, goToLocation, Time.deltaTime * movementSpeed);
                if (transform.position == goToLocation) {
                    inPosition = true;
                }
            }
            else {
                inPosition = true;
            }
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (col.tag == "Player" && inPosition) {
            //col.gameObject.GetComponent<PlayerChar>().HP += 1;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ChangePoints(points);
            col.GetComponent<PlayerChar>().audioSource.PlayOneShot(col.GetComponent<PlayerChar>().pickupCandy);
            Destroy(this.gameObject);
        }
    }

    IEnumerator RotateCandy() {
        while (true) {
            transform.Rotate(0, rotationSpeed, 0);
            yield return null;
        }
    }

    IEnumerator RotateCandy2() {
        //isMopping = true;
        while (true) {
            //while (state == AIState.mopping) {
            transform.Rotate(0, 0, (Mathf.Sin(rotateTimer * rotationSpeed) - Mathf.Sin(prevRotateTimer * rotationSpeed)) * 20);
            prevRotateTimer = rotateTimer;
            rotateTimer += Time.deltaTime;
            yield return null;
        }
        //}
        //isMopping = false;
    }
}
