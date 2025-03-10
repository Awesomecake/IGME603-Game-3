using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Fading")]
    [SerializeField] bool waitToLerp = true;
    [SerializeField] float menuAlphaThreshold = 0.2f;
    [SerializeField] float fadeSpeed = 1.0f;

    [Header("Cursor Settings")]
    [SerializeField] Texture2D cursorOne;
    [SerializeField] Texture2D cursorTwo;


    private bool canLerp = false;
    
    private CanvasGroup from, to;

    private void Start() 
    {
        // Check if the current scene is the MainMenu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Cursor.SetCursor(cursorOne, new Vector2(64, 64), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorTwo, new Vector2(64, 64), CursorMode.Auto);
        }
    }

    private void Update()
    {
        // Fading Menus
        if (canLerp)
        {
            from.alpha -= Time.unscaledDeltaTime * fadeSpeed;
            if(from.alpha <= menuAlphaThreshold && waitToLerp || !waitToLerp)
            {
                to.gameObject.SetActive(true);
                to.alpha += Time.unscaledDeltaTime  * fadeSpeed;
            }

            if (from.alpha == 0)
            {
                from.gameObject.SetActive(false);
            }
            if (to.alpha == 1)
            {
                canLerp = false;
            }
        };
    }

    // Fading Menu Functions
    public void SetFrom(CanvasGroup From) => from = From;

    public void SetTo(CanvasGroup To)
    {
        to = To;
        to.alpha = 0;
    } 
    public void ChangeMenu() => canLerp = true;

    // Scene Managment

    public void ChangeSceneFromIndex(int sceneIndex) => SceneTransitoner.Instance.StartTransitionScene(sceneIndex); 
    public void ChangeSceneFromName(string sceneName) => SceneTransitoner.Instance.StartTransitionScene(sceneName); 
    public void Quit() => Application.Quit();
}
