using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject subMenu;

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

    public void ToggleVisibilitySubMenu()
    {
        visibility = !visibility;
        subMenu.SetActive(visibility);

        if (visibility)
            subMenuBtn.GetComponent<Image>().sprite = spritePressed;
        else
            subMenuBtn.GetComponent<Image>().sprite = spriteNotPressed;
    }

    public void ClickOnPause()
    {
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
        print("Quit");
        Application.Quit();
    }
}
