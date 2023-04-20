using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInGame : MonoBehaviour
{
    [SerializeField]
    private GameSystem _game;
    private TurnManager _turn => _game.TurnManager;

    [SerializeField]
    private PlayerStatUI _playerStat, _opponentStat, _statPerTurn;

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

    [SerializeField]
    private GameObject startButton; // clickable only once

    private bool visibility;
    private bool gameIsPaused;
    private bool hasStarted;

    public UnityEvent OnGameStart;

    [Header("Sound")]
    [SerializeField]
    private SoundManager soundManager;
    public AudioClip pauseSound;
    public AudioClip subSound;
    public AudioClip nextSound;

    [SerializeField]
    private GameObject endMenu;
    [SerializeField]
    private TextMeshProUGUI endSentence;

    private void Awake()
    {
        if (OnGameStart == null)
            OnGameStart = new();
        WhosPlaying.onClick.AddListener(_turn.SwitchTurn);
    }

    // On start, pausing so that we can play only once we clicked on start button
    private void Start()
    {
        PauseGame();
        WhosPlaying.gameObject.SetActive(false);
        endMenu.SetActive(false);

        _game.OnPlayerLost.AddListener(OnPlayerLost);
        _game.OnPlayerValuesUpdates.AddListener(OnPlayerUpdates);
        _turn.TurnChanged.AddListener((b) =>
        {
            if (b) BeginTurn(); else FinishTurn();
        });
    }

    public void OnPlayerUpdates((int, GameSystem.Player p) player)
    {
        (player.Item1 == 0 ? _playerStat : _opponentStat).UpdateView(player.p);
        _statPerTurn.UpdateView(_game.GetResourcesForNextRound(player.p));
    }

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
        if (!hasStarted)
            return;

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
    //Call from button
    private void FinishTurn()
    {
        soundManager.PlaySound(nextSound);
        // WhosPlaying.enabled = false;
        WhosPlaying.GetComponent<Image>().sprite = notYourTurn;
    }
    //Call from bot
    private void BeginTurn()
    {
        soundManager.PlaySound(nextSound);
        WhosPlaying.enabled = true;
        WhosPlaying.GetComponent<Image>().sprite = yourTurn;
    }

    public void StartGame()
    {
        OnGameStart.Invoke();
        ResumeGame();
        WhosPlaying.gameObject.SetActive(true);
        startButton.SetActive(false);
        hasStarted = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // since game is 1vs1 only, if one player lost, the game is over
    public void OnPlayerLost(int iPlayerIndexLost)
    {
        subMenuBtn.gameObject.SetActive(false);
        WhosPlaying.gameObject.SetActive(false);
        endMenu.SetActive(true);

        if (iPlayerIndexLost == PlayerID.IsPlayerAsInt(true))
            endSentence.text = "You failed to sustainably manage your planet.";
        else
            endSentence.text = "Sustainable development has no secret for you, congrats !";
    }
}
