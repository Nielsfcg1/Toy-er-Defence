using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    private void OnTriggerEnter(Collider col) {
        if (col.tag == "Bullet") {
            Destroy(col.gameObject);
        }
    }


}
