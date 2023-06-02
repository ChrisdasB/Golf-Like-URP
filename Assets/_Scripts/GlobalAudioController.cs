using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioController : MonoBehaviour
{
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Event subs
        GameManager.PlayStage += AudioFull;
        GameManager.PauseStage += AudioLowered;
    }

    // Lower audio-volume in pause menu
    private void AudioLowered()
    {
        audioSource.volume = 0.35f;
    }

    // Set Audio to 100% when entering GameState Play
    private void AudioFull()
    {
        audioSource.volume = 1f;
    }
}

