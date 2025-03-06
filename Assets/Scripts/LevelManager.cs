using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
}