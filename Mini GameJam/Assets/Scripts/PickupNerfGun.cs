using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupNerfGun : MonoBehaviour {
    public int rotationSpeed = 4;
    public bool inPosition = false;
    public Vector3 goToLocation;
    public float movementSpeed = 1;
    public LayerMask layerMask;

    void Start() {
        StartCoroutine(RotatePickup());
    }

    // Animate the gun falling out of your hands, move it to where it will be dropped. It cannot be picked during this time until it is at its destination. If it hits a wall and is stopped, that will be its destination.
    void Update() {
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

    public void Initialize(Vector3 endPos) {
        goToLocation = endPos;
    }

    private void OnTriggerEnter(Collider col) {
        if (col.tag == "Player" && inPosition) {
            //col.gameObject.GetComponent<PlayerChar>().HP += 1;
            col.gameObject.GetComponent<PlayerChar>().gunPossessed = true;
            Destroy(this.gameObject);
        }
    }

    IEnumerator RotatePickup() {
        while (true) {
            transform.Rotate(0, 0, rotationSpeed);
            yield return null;
        }
    }
}
