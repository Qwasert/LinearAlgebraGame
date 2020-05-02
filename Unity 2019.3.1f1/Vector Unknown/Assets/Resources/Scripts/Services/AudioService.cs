﻿using UnityEngine;

public class AudioService : MonoBehaviour
{
    public AudioSource audioBg;
    public AudioSource audioUI;
    public AudioSource audioFX;
    public AudioSource audioPlayerMove;

    public void InitService()
    {
        Debug.Log("Init Audio Service");
    }

    public void PlayBgMusic(string audioName, bool isLoop)
    {
        //Get the audio clip by the resource service
        AudioClip audioClip = GameRoot.instance.resourceService.LoadAudio("Audios/Musics/" + audioName, true);

        //Check if the background audio clip whether is null or next audio clip name matches the current audio clip
        if (audioBg.clip == null || audioBg.clip.name != audioClip.name)
        {
            audioBg.clip = audioClip;
            audioBg.loop = isLoop;
            audioBg.Play();
        }
    }

    public void PlayUIAudio(string audioName)
    {
        //Get the audio clip by the resource service
        AudioClip audioClip = GameRoot.instance.resourceService.LoadAudio("Audios/Sound FX/" + audioName, true);
        audioUI.clip = audioClip;
        audioUI.Play();
    }

    public void PlayFXAudio(string audioName)
    {
        //Get the audio clip by the resource service
        AudioClip audioClip = GameRoot.instance.resourceService.LoadAudio("Audios/Sound FX/" + audioName, true);
        audioFX.clip = audioClip;
        audioFX.Play();
    }

    public AudioClip GetFXAudioClip(string audioName)
    {
        return GameRoot.instance.resourceService.LoadAudio("Audios/Sound FX/" + audioName, false);
    }

    public void PauseAllAudios()
    {
        audioBg.volume *= 0.2f;
    }

    public void ResumeAllAudios()
    {
        audioBg.volume *= 5f;
    }

    public void SetBgVolume(float volume)
    {
        if(GameRoot.isPause)
        {
            audioBg.volume = volume * 0.5f * 0.2f;
        }
        else
        {
            audioBg.volume = volume * 0.5f;
        }
    }

    public void SetSoundFXVolume(float volume)
    {
        audioUI.volume = volume;
        audioFX.volume = volume;
    }
}
