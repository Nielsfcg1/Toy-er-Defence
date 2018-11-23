using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyBearStartmenu : MonoBehaviour {

    Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        animator.Play("TeddyBearClose");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
