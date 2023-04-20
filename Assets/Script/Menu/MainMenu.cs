using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Image volumeImage;
    [SerializeField]
    private Sprite spriteMute;
    [SerializeField]
    private Sprite spriteDemute;

    [SerializeField]
    private AudioSource cowAudio;

    public void Start()
    {
        LoadVolume();
    }

    public void ChangeVolume()
    {
        float volume = volumeSlider.value;
        // set sound in game

        PlayerPrefs.SetFloat("musicVolume", volume);

        if(volume > 0)
            volumeImage.sprite = spriteDemute;
        else
            volumeImage.sprite = spriteMute;
        
        Camera.main.GetComponent<AudioSource>().volume = volume;
        cowAudio.volume = volume;
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");

        ChangeVolume();
    }

    public void Quit()
    {
        print("Quit");
        Application.Quit();
    }
}
