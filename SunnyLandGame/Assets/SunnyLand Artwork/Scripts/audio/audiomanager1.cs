using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiomanager1 : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioClip backgroundSound;
    void Start()
    {
        PlayBackgroundSound();

    }

    void Update()
    {
        
    }

    private void PlayBackgroundSound()
    {
        backgroundAudioSource.clip = backgroundSound;
        backgroundAudioSource.Play();
    }
}
