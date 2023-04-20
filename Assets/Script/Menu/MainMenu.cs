using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private Slider volumeSliderSFX;
    [SerializeField]
    private Slider volumeSliderMusic;

    [SerializeField]
    private Image volumeImageSFX;
    [SerializeField]
    private Image volumeImageMusic;
    [SerializeField]
    private Sprite spriteMute;
    [SerializeField]
    private Sprite spriteDemute;

    [Header("Sound")]
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private AudioSource cowAudio;
    [SerializeField]
    private AudioSource MusicManager;
    [SerializeField]
    private AudioSource SFXManager;
    public AudioClip volumeChangeSound;
    public AudioClip buttonSound;


    public void Start()
    {
        LoadVolume();
    }

    public void ChangeVolumeSFX()
    {
        float volumeSFX = volumeSliderSFX.value;
        // set sound in game

        PlayerPrefs.SetFloat("musicVolumeSFX", volumeSFX);

        if(volumeSFX > 0)
            volumeImageSFX.sprite = spriteDemute;
        else
            volumeImageSFX.sprite = spriteMute;
        
        cowAudio.volume = volumeSFX;
        SFXManager.volume = volumeSFX;
        soundManager.PlaySound(volumeChangeSound);
    }

    public void ChangeVolumeMusic()
    {
        float volumeMusic = volumeSliderMusic.value;
        // set sound in game

        PlayerPrefs.SetFloat("musicVolumeMusic", volumeMusic);

        if (volumeMusic > 0)
            volumeImageMusic.sprite = spriteDemute;
        else
            volumeImageMusic.sprite = spriteMute;

        MusicManager.volume = volumeMusic;
        soundManager.PlaySound(volumeChangeSound);
    }

    private void LoadVolume()
    {
        volumeSliderSFX.value = PlayerPrefs.GetFloat("musicVolumeSFX");
        volumeSliderMusic.value = PlayerPrefs.GetFloat("musicVolumeMusic");

        ChangeVolumeSFX();
        ChangeVolumeMusic();
    }

    public void Quit()
    {
        soundManager.PlaySound(buttonSound);
        print("Quit");
        Application.Quit();
    }
}
