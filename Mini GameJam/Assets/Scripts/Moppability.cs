using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moppability : MonoBehaviour {

    public float mopFinishedAt = 2f;
    public float mopProgress = 0f;
    //public bool isBeingMopped = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (mopProgress > mopFinishedAt) {
            Debug.Log("pool of blood has finished being mopped");
            //Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider col) {
        StartMopping(col);
    }

    private void OnTriggerEnter(Collider col) {
        StartMopping(col);

    }

    void StartMopping(Collider col) {
        if (col.tag == "Mother" || col.tag == "Father") {
            col.GetComponent<Parent>().StartMopping(this);
        }
    }

    IEnumerator CheckWhetherMopped() {
        yield return new WaitForSeconds(mopFinishedAt);
        //isBeingMopped = false;
    }
}