using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public static UIController Instance;

    public GameObject gameOverScreen;

    void Awake()
    {
        Instance = this;
    }
}