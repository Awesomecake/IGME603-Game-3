using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public bool pauseOnKeypress = true;
    public GameObject toolScreen;
    public GameObject winScreen;
    public TextMeshProUGUI timeToBeatText;
    public TextMeshProUGUI winStreakText;
    public GameObject loseScreen;
    public TextMeshProUGUI loseTitleText;
    public TextMeshProUGUI streakLostText;
    public TextMeshProUGUI totalWinsText;
    public TextMeshProUGUI totalLossesText;
    [SerializeField] private GameObject pauseMenu;
    
    [SerializeField] private bool freezeTime = true;
    [SerializeField] private string mainMenuScene = "MainMenu";

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (!pauseOnKeypress) return;
        HandlePauseButtonPressed();
    }

    private void HandlePauseButtonPressed()
    {
        var pressedPause = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7);
        if (!pressedPause) return;
        if (!pauseMenu.activeSelf) Pause();
        else Unpause();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        if (freezeTime) Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void MainMenu()
    {
        Unpause();
        // Reset Current Total Wins and Losses
        PlayerPrefs.SetInt("Wins", 0);
        PlayerPrefs.SetInt("Loses", 0);
        SceneManager.LoadScene(mainMenuScene);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}