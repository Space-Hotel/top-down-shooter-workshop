using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource backgroundMusic;
    private AudioSource gameOverMusic;
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        backgroundMusic = audioSources[0];
        gameOverMusic = audioSources[1];
    }

    public void PlayGameOver()
    {
        backgroundMusic.Stop();
        gameOverMusic.Play();
    }
}
