using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Gameplay Stats")]
    public int wins = 0;
    public int winStreak = 0;
    public int bestWinStreak = 0;
    public int loses = 0;
    public int timeToComplete = 0;

    [Header("Components")]
    public static LevelManager Instance;
    [SerializeField] private PauseManager pauseManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        LoadGameplayStats();
    }

    public UnityEvent onPlayerCollectDiamond;
    public UnityEvent onBroCollectDiamond;
    public UnityEvent onPlayerDie;

    [HideInInspector] public GameObject player;

    public void NotifyPlayerCollectedDiamond()
    {
        onPlayerCollectDiamond?.Invoke();
    }
    
    public void NotifyBroCollectedDiamond()
    {
        onBroCollectDiamond?.Invoke();
    }
    
    public void NotifyPlayerDie()
    {
        onPlayerDie?.Invoke();
    }

    public void RegisterPlayer(GameObject playerToRegister)
    {
        player = playerToRegister;
    }

    public void ReloadScene()
    {
        Destroy(player);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WinLevel()
    {
        // Update Stats
        // TODO: Add timeToBeat implementation
        wins++;
        winStreak++;

        // Update Screen
        Time.timeScale = 0;
        pauseManager.pauseOnKeypress = false;
        if(winStreak > bestWinStreak)
        {
            bestWinStreak = winStreak;
            pauseManager.winStreakText.text = $"Win Streak: {winStreak} (<rainb w=0.5 f=1>New Best!</rainb>)";
        }else pauseManager.winStreakText.text = $"Win Streak: {winStreak}";
        SaveGameplayStats();

        pauseManager.winScreen.SetActive(true);
    }

    public void LoseLevel(string method)
    {
        // Update Stats
        if(winStreak != 0)
        {
            pauseManager.streakLostText.gameObject.SetActive(true);
            winStreak = 0;
        }else
        {
            pauseManager.streakLostText.gameObject.SetActive(false);
        } 
        loses++;
        SaveGameplayStats();

        // Update Screen
        Time.timeScale = 0;
        pauseManager.pauseOnKeypress = false;
        pauseManager.loseTitleText.text = method; // Set Custom Lose Message
        pauseManager.totalWinsText.text = $"Wins: {wins}";
        pauseManager.totalLossesText.text = $"Losses: {loses}";
        pauseManager.loseScreen.SetActive(true);
    }

    private void SaveGameplayStats()
    {
        PlayerPrefs.SetInt("Wins", wins);
        PlayerPrefs.SetInt("WinStreak", winStreak);
        PlayerPrefs.SetInt("BestWinStreak", bestWinStreak);
        PlayerPrefs.SetInt("Loses", loses);

        PlayerPrefs.Save();
    }

    private void LoadGameplayStats()
    {
        wins = PlayerPrefs.GetInt("Wins", 0);
        winStreak = PlayerPrefs.GetInt("WinStreak", 0);
        bestWinStreak = PlayerPrefs.GetInt("BestWinStreak", 0);
        loses = PlayerPrefs.GetInt("Loses", 0);
    }
}