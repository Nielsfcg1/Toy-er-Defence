using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyBear : MonoBehaviour {

    public static TeddyBear Instance;

    public enum AnimState { idle, close, pickup }
    public AnimState animState;

    Animator animator;

    public AudioClip pickupAudioClip;

    AudioSource audioSource;

    public float weight;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetAnimIdle()
    {
        if (animState != AnimState.idle)
        {
            animator.Play("TeddyBearIdle");
            animState = AnimState.idle;
        }
    }

    public void SetAnimClose()
    {
        if (animState != AnimState.close)
        {
            animator.Play("TeddyBearClose");
            animState = AnimState.close;
        }
    }

    public void SetAnimPickup()
    {
        if (animState != AnimState.pickup)
        {
            animator.Play("TeddyBearPickUp");
            animState = AnimState.pickup;
            audioSource.PlayOneShot(pickupAudioClip);
        }
    }
}