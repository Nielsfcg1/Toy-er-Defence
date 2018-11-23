using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public int cost;
    public int weight;

    public bool canBuild;

    /*void Update()
    {
        spriteRenderer.sortingOrder = (int)(-transform.position.z * 100);
    }*/

    void Start()
    {
        canBuild = true;
    }
    
    void OnCollisionStay(Collision col)
    {
        if (col.transform.tag == "Wall")
            canBuild = false;
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    if(col.transform.tag == "Wall")
    //        canBuild = false;
    //}

    void OnCollisionExit(Collision col)
    {
        if (col.transform.tag == "Wall")
            canBuild = true;
    }
}