using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private bool pauseOnKeypress = true;
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
        var pressedPause = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7);
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
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}