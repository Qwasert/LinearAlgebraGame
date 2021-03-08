using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleWindow : WindowRoot
{
    public Transform panelMain;
    public Transform panelOptions;
    public Slider sliderVolume;
    public Slider sliderSoundFX;
    public Dropdown ddResolution;

    //private Resolution[] resolutions;

    protected override void InitWindow()
    {
        base.InitWindow();

        SetBgVolume();

        SetSoundFXVolume();

        // show mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


        //Initialize Resolution Dropdown
        /*
        resolutions = Screen.resolutions;
        ddResolution.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            if((double)resolutions[i].width/resolutions[i].height == 16.0/9.0)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
        }

        ddResolution.AddOptions(options);
        ddResolution.value = currentResolutionIndex;
        ddResolution.RefreshShownValue();*/
        //*************************************
    }

    public void ClickPlayBtn()
    {
        //Reset puzzle complete status
        for (int i = 0; i < GameRoot.instance.puzzleCompleted.Length; i++)
        {
            GameRoot.instance.puzzleCompleted[i] = false;
        }

        DialogueManager.instance.ResetAll();

        audioService.PlayUIAudio(Constants.audioUIStartBtn);
        //GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle01SceneName);// original  
        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.mainSceneName);//added by LaQuez Brown 
        

        // hides mouse when loading game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void ClickTutorialBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.tutorialSceneName);
    }

    public void ClickOptionsBtn()
    {
        SetActive(panelMain, false);
        SetActive(panelOptions, true);
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
    }

    public void ClickQuitBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        Application.Quit();
    }

    public void SetBgVolume()
    {
        audioService.SetBgVolume(sliderVolume.value);
    }

    public void SetSoundFXVolume()
    {
        audioService.SetSoundFXVolume(sliderSoundFX.value);
    }

    public void ClickBackBtn()
    {
        SetActive(panelOptions, false);
        SetActive(panelMain, true);
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex+1);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        
    }

    public void SetResolution(int index)
    {
        //Resolution resolution = resolutions[index];
        //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        //Screen.SetResolution(1920, 1080, false);
    }
}
