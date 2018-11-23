using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverFightBack : MonoBehaviour {
    
    //distance from child until the mover knocks the gun out
    public float childHitDistance;

    public bool canHitGun = true;

    // Update is called once per frame
    void Update () {
        if (Vector3.Distance(transform.position, PlayerChar.Instance.transform.position) < childHitDistance && canHitGun) {
            if (PlayerChar.Instance.gunEquipped) {
                PlayerChar.Instance.DropWeapon();
            }
        }
    }
}
