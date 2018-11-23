using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public float movementSpeed;
    public GameObject gun;
    public Vector3 prevPos;
    public LayerMask walls;

    SpriteRenderer sr;

    AudioSource audioSource;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();
    }


    void Update() {
        prevPos = transform.position;

        if (Input.GetKey(KeyCode.W)) {
            if (!Physics.Raycast(transform.position, Vector3.forward, Time.deltaTime * movementSpeed, walls)) {
                transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
            }
        }
        else if (Input.GetKey(KeyCode.S)) {
            if (!Physics.Raycast(transform.position, Vector3.back, Time.deltaTime * movementSpeed, walls)) {
                transform.Translate(Vector3.down * Time.deltaTime * movementSpeed);
            }
        }

        if (Input.GetKey(KeyCode.A)) {
            if (!Physics.Raycast(transform.position, Vector3.left, Time.deltaTime * movementSpeed, walls)) {
                transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
            }
        }
        else if (Input.GetKey(KeyCode.D)) {
            if (!Physics.Raycast(transform.position, Vector3.right, Time.deltaTime * movementSpeed, walls)) {
                transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
            }
        }

        //Move buildable object towards mouse position
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(gun.transform.position);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(angle) < 90) {
            gun.transform.rotation = Quaternion.Euler(90, 0, angle);
        }
        else {
            gun.transform.rotation = Quaternion.Euler(-90, 0, -angle);
        }
        sr.sortingOrder = (int)(-transform.position.z * 100);
    }

    /*private void OnTriggerStay(Collider collision) {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Wall") {
            transform.Translate((transform.position - prevPos) * -2);
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }
    }*/
}
