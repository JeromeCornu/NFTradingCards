using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    [SerializeField]
    private GameObject subMenu;

    [SerializeField]
    private Button WhosPlaying;
    [SerializeField]
    private Sprite yourTurn;
    [SerializeField]
    private Sprite notYourTurn;

    [SerializeField]
    private Button subMenuBtn;
    [SerializeField]
    private Sprite spritePressed;
    [SerializeField]
    private Sprite spriteNotPressed;

    [SerializeField]
    private Button pauseBtn;
    [SerializeField]
    private Sprite spritePause;
    [SerializeField]
    private Sprite spritePlay;

    private bool visibility;
    private bool gameIsPaused;


    [SerializeField]
    private SoundManager soundManager;
    public AudioClip pauseSound;
    public AudioClip subSound;
    public AudioClip nextSound;

    public void ToggleVisibilitySubMenu()
    {
        soundManager.PlaySound(subSound);

        visibility = !visibility;
        subMenu.SetActive(visibility);

        if (visibility)
            subMenuBtn.GetComponent<Image>().sprite = spritePressed;
        else
            subMenuBtn.GetComponent<Image>().sprite = spriteNotPressed;
    }

    public void ClickOnPause()
    {
        soundManager.PlaySound(pauseSound);

        if (gameIsPaused)
            ResumeGame();
        else
            PauseGame();

        gameIsPaused = !gameIsPaused;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        pauseBtn.GetComponent<Image>().sprite = spritePause;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        pauseBtn.GetComponent<Image>().sprite = spritePlay;
    }

    public void Quit()
    {
        soundManager.PlaySound(pauseSound);
        print("Quit");
        Application.Quit();
    }

    public void FinishTurn()
    {
        soundManager.PlaySound(nextSound);
        WhosPlaying.enabled = false;
        WhosPlaying.GetComponent<Image>().sprite = notYourTurn;
    }

    public void BeginTurn()
    {
        soundManager.PlaySound(nextSound);
        WhosPlaying.enabled = true;
        WhosPlaying.GetComponent<Image>().sprite = yourTurn;
    }
}
