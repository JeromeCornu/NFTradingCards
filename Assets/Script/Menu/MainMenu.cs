using System;
using System.Collections;
using System.Collections.Generic;
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

    public void Start()
    {
        LoadVolume();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
