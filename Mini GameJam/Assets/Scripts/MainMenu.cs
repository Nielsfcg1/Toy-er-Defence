using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public AudioSource audioSource;

    public AudioClip click;

    public void Play()
    {
        StartCoroutine(WaitOneSecond());
    }

    public void ExitGame()
    {
        audioSource.PlayOneShot(click);
        Application.Quit();
    }

    IEnumerator WaitOneSecond()
    {
        audioSource.PlayOneShot(click);

        yield return new WaitForSeconds(1);

        if (PlayerPrefs.GetInt("ExplanationDone") == 1)
        {
            SceneManager.LoadScene(1);
        }
        else if (PlayerPrefs.GetInt("ExplanationDone") == 0)
        {
            SceneManager.LoadScene(2);
        }
    }

}
