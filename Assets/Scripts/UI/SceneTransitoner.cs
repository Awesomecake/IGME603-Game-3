using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneTransitoner : MonoBehaviour
{
    public static SceneTransitoner Instance;

    [Header("Components")]
    public Animator transitonAnimator;

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
        transitonAnimator.Play("FadeOut");
    }

    public void StartTransitionScene(int sceneIndex)
    {
        StartCoroutine(TransitionScene(sceneIndex));
    }
    public void StartTransitionScene(string sceneName)
    {
        StartCoroutine(TransitionScene(sceneName));
    }
    public void StartTransitionMenu(GameObject menu)
    {
        StartCoroutine(TransitionMenu(menu));
    }

    private IEnumerator TransitionScene(int sceneIndex)
    {
        transitonAnimator.Play("FadeIn");
        yield return null;
        yield return new WaitForSecondsRealtime(SceneTransitoner.Instance.transitonAnimator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(sceneIndex);
    }
    private IEnumerator TransitionScene(string sceneName)
    {
        transitonAnimator.Play("FadeIn");
        yield return null;
        yield return new WaitForSecondsRealtime(SceneTransitoner.Instance.transitonAnimator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(sceneName);
    }
    private IEnumerator TransitionMenu(GameObject menu)
    {
        transitonAnimator.Play("FadeIn");
        yield return null;
        yield return new WaitForSecondsRealtime(SceneTransitoner.Instance.transitonAnimator.GetCurrentAnimatorStateInfo(0).length);
        menu.SetActive(true);
        transitonAnimator.Play("FadeOut");
    }

}
