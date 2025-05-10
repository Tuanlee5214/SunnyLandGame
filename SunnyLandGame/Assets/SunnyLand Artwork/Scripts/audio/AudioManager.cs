using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip backgroundSound;
    [SerializeField] private AudioClip ItemSound;
    [SerializeField] private AudioClip HurtSound;
    [SerializeField] private AudioClip KillEnemySound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip gameWinSound;
    [SerializeField] private AudioClip bossHurtSound;
    [SerializeField] private AudioClip bossAttack;
    private GameManager gameManager;
    //[SerializeField] private AudioClip OntheGroundSound;

    void Start()
    {
        PlayBackgroundSound();
    }


    void Update()
    {
        

    }

    public void PlayBackgroundSound()
    {
        backgroundAudioSource.clip = backgroundSound;
        backgroundAudioSource.Play();
    }

    public void PlayItemSound()
    {
        effectAudioSource.PlayOneShot(ItemSound);
    }

    public void PlayHurtSound()
    {
        effectAudioSource.PlayOneShot(HurtSound);
    }

    public void PlayJumpSound()
    {
        effectAudioSource.PlayOneShot(jumpSound);
    }

    public void PlayKillEnemySound()
    {
        effectAudioSource.PlayOneShot(KillEnemySound);
    }
    public void PlayBossAttack()
    {
        effectAudioSource.PlayOneShot(bossAttack);
    }
    public void PlayGameOverSound()
    {
        effectAudioSource.PlayOneShot(gameOverSound);
    }

    public void PlayGameWinSound()
    {
        effectAudioSource.PlayOneShot(gameWinSound);
    }

    public void PlayBossHurtSound()
    {
        effectAudioSource.PlayOneShot(bossHurtSound);
    }
    public void PauseBackgroundAudioSource()
    {
        backgroundAudioSource.clip = backgroundSound;
        backgroundAudioSource.Pause();
    }

   
}
