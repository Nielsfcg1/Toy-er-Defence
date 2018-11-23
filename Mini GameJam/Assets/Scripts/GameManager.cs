using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    bool paused = false;
	public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public WaveController waveController;
    public Text gameOverText;
    public int points = 100;

    public AudioSource click;

    public Text candyCounterText;


    public int playerHealth;
    public List<Image> hearts;

    void Start()
    {
        ResumeGame();
        candyCounterText.text = points.ToString();
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            click.Play();

            if (paused)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

            pauseMenu.SetActive(paused);
        }
       
	}

    public void ResumeGame()
    {
        Time.timeScale = 1;
        paused = false;
        pauseMenu.SetActive(false);
    }

    public void LoadMainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);        
    }

    public void ChangePoints(int amount)
    {
        points += amount;
        candyCounterText.text = points.ToString();
    }

    public void ChangeHP(int hp)
    {
        for(int i = hp; i<3; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
    }

    public void GameOver()
    {
        waveController.MusicSource.Stop();
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
        gameOverMenu.GetComponent<AudioSource>().Play();
    }
}
