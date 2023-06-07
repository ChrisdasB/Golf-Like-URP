using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Create Event
    public static event Action AudioValuesSet;

    [SerializeField] AudioMixer masterAudioMixer;
    [SerializeField] Slider masterAudioSlider;
    [SerializeField] Slider musicAudioSlider;
    [SerializeField] Slider sfxAudioSlider;
    [SerializeField] Button saveAudioBtn;

    string masterParamString = "MasterVolume";
    string musicParamString = "MusicVolume";
    string sfxParamString = "SFXVolume";

    bool blendOutAudio = false;
    bool blendInAudio = false;

    // Start is called before the first frame update
    void Awake()
    {
        // Event subs
        masterAudioSlider.onValueChanged.AddListener(HandleMasterAudioChanged);
        musicAudioSlider.onValueChanged.AddListener(HandleMusicAudioChanged);
        sfxAudioSlider.onValueChanged.AddListener(HandleSFXAudioChanged);

        saveAudioBtn.onClick.AddListener(SaveAudioSettings);
        DataManagerSingleton.DataLoaded += SetInitialAudio;
        DataManagerSingleton.CloseScene += ClosingScene;
        MySceneManager.SceneIsLoaded += OpeningScene;
    }

    private void OnDestroy()
    {
        DataManagerSingleton.DataLoaded -= SetInitialAudio;
        DataManagerSingleton.CloseScene -= ClosingScene;
        MySceneManager.SceneIsLoaded -= OpeningScene;
    }



    private void SetInitialAudio()
    {
        masterAudioSlider.minValue = -80;
        masterAudioSlider.maxValue = 0;
        sfxAudioSlider.minValue = -80;
        sfxAudioSlider.maxValue = 0;
        musicAudioSlider.minValue = -80;
        musicAudioSlider.maxValue = 0;
    }

    private void Start()
    {
        HandleMasterAudioChanged(DataManagerSingleton.savedData.totalAudioValue);
        HandleMusicAudioChanged(DataManagerSingleton.savedData.musicAudioValue);
        HandleSFXAudioChanged(DataManagerSingleton.savedData.sfxAudioValue);
        if(MySceneManager.currentScene != 1)
        {
            AudioValuesSet.Invoke();
        }
        
    }

    private void SaveAudioSettings()
    {
        DataManagerSingleton.Instance.SaveAudioData(masterAudioSlider.value, musicAudioSlider.value, sfxAudioSlider.value);
    }

    private void HandleMasterAudioChanged(float newVolume)
    {
        // Set new volume
        masterAudioSlider.value = newVolume;
        masterAudioMixer.SetFloat(masterParamString, newVolume);
        print("Attempted to set master audio to: " + newVolume);        
    }

    private void HandleMusicAudioChanged(float newVolume)
    {
        musicAudioSlider.value = newVolume;
        masterAudioMixer.SetFloat(musicParamString, newVolume);        
    }

    private void HandleSFXAudioChanged(float newVolume)
    {
        sfxAudioSlider.value = newVolume;
        masterAudioMixer.SetFloat(sfxParamString, newVolume);        
    }

    void ClosingScene()
    {
        blendOutAudio = true;
    }

    private void OpeningScene()
    {
        blendInAudio = true;
    }

    private void Update()
    {
        if(blendOutAudio) 
        {
            if (musicAudioSlider.value <= musicAudioSlider.minValue)
            {
                blendOutAudio = false;
            }
            HandleMusicAudioChanged(musicAudioSlider.value - 0.1f);

            
        }

        if (blendInAudio)
        {
            if (musicAudioSlider.value >= DataManagerSingleton.savedData.musicAudioValue)
            {
                blendInAudio = false;
                HandleMusicAudioChanged(DataManagerSingleton.savedData.musicAudioValue);
            }

            HandleMusicAudioChanged(musicAudioSlider.value + 0.3f);

            
        }
    }

}
