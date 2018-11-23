using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveController : MonoBehaviour {

    public static WaveController Instance;

    public Wave[] waves;
    public AudioClip[] music;
    public AudioSource MusicSource;
    public GameObject exit;

    int currentWave;

    public int nextWaveWaitTime;
    public bool fadeIn;

    Coroutine waitForNextWaveRoutine;

    float waveTimer;
    bool waitingNextWave;
    public Text waitingTimeUI;
    public Text waveCounterUI;

    public GameObject motherObj;
    public GameObject fatherObj;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        NextWave();
        MusicSource.volume = 0f;
    }

    void Update()
    {
        if (waitingNextWave)
        {
            waveTimer -= Time.deltaTime;
            waitingTimeUI.text = "Next wave " + waveTimer.ToString("0");
        }

        if(MusicSource.volume <0.2f)
        {
            musicFadeIn();
        }
            

        if (MoverController.Instance.movers.Count <= 0)
        {
            if (waitForNextWaveRoutine == null)
            {
                waitForNextWaveRoutine = StartCoroutine(WaitForNextWave());
                waitingNextWave = true;
                waveTimer = 20;
                waitingTimeUI.gameObject.SetActive(true);
                fadeOut();
            }
        }
    }

    public void NextWave()
    {
        if (currentWave >= 9)
        {
            waveCounterUI.text = "You have won the game!";
            return;
        }
        StartCoroutine(SpreadMoverStart());
        fadeIn = true;
        MusicSource.volume = 0f;
        MusicSource.PlayOneShot(music[Random.Range(0, music.Length)]);

    }

    public void musicFadeIn()
    {
        MusicSource.volume += 0.01f * Time.deltaTime;
    }

    public void fadeOut()
    {
        MusicSource.volume -= 0.1f * Time.deltaTime;
    }

    IEnumerator WaitForNextWave()
    {
        yield return new WaitForSeconds(20);

        NextWave();
        waitingNextWave = false;
        waitingTimeUI.gameObject.SetActive(false);

        waitForNextWaveRoutine = null;
    }

    IEnumerator SpreadMoverStart()
    {
        fatherObj.SetActive(false);
        motherObj.SetActive(false);

        for (int i = 0; i < waves[currentWave].amtMovers; i++)
        {
            int possibleMoverLength = waves[currentWave].possibleMovers.Length;
            GameObject moverInst = Instantiate(waves[currentWave].possibleMovers[Random.Range(0, possibleMoverLength)]);
            moverInst.name = "Mover " + (i + 1);

            moverInst.transform.position = exit.transform.position;

            Mover mover = moverInst.GetComponent<Mover>();

            mover.exit = exit.transform;

            MoverController.Instance.movers.Add(mover);

            yield return new WaitForSeconds(0.5f);
        }



        if (waves[currentWave].hasFather)
        {
            fatherObj.SetActive(true);
            fatherObj.transform.position = exit.transform.position;
        }
        if (waves[currentWave].hasMother)
        {
            motherObj.SetActive(true);
            motherObj.transform.position = exit.transform.position;
        }

        currentWave++;
        waveCounterUI.text = "Current wave: " + currentWave;
        if (currentWave >= 9) {
            waveCounterUI.text = "You have won the game!";
        }
    }
}