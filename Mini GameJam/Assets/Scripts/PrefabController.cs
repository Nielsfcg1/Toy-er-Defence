using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour {

    public static PrefabController Instance;

    public GameObject droppedExitObject;

    void Awake()
    {
        Instance = this;
    }
}