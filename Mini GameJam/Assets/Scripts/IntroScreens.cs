using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreens : MonoBehaviour {

    int currentState;

    public GameObject screen1;
    public GameObject screen2;
    public GameObject screen3;
    public GameObject screen4;
    public GameObject screen5;

    public AudioSource source;
    public AudioClip click;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Next();
        }
    }

    public void Next()
    {
        source.PlayOneShot(click);

        if (currentState == 0)
        {
            screen1.SetActive(false);
            screen2.SetActive(true);
        }
        else if (currentState == 1)
        {
            screen2.SetActive(false);
            screen3.SetActive(true);
        }
        else if (currentState == 2)
        {
            screen3.SetActive(false);
            screen4.SetActive(true);
        }
        else if (currentState == 3)
        {
            screen4.SetActive(false);
            screen5.SetActive(true);
        }
        else if (currentState == 4)
        {
            PlayerPrefs.SetInt("ExplanationDone", 1);
            SceneManager.LoadScene(1);
        }

        currentState++;
    }
}