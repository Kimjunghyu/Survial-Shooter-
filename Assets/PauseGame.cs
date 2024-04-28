using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PauseGame : MonoBehaviour
{
    public GameObject go;
    private AudioSource[] allAudioSources;
    public AudioSource[] EffectAudioSources;
    public AudioSource BgmSources;
    void Start()
    {
        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allAudioSources)
        {
            source.GetComponent<AudioSource>();
        }
        BgmSources = GetComponent<AudioSource>();
    }
    public void SetAllAudioVolume(float volume)
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = volume;
        }
    }
    public void SetEffectVolume(float volume)
    {
        foreach(AudioSource audioSource in EffectAudioSources)
        {
            audioSource.volume = volume;
        }
    }
    public void SetBgmVolume(float volume)
    {
        BgmSources.volume = volume;
    }

    public void Quit()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        go.SetActive(false);
    }
}
