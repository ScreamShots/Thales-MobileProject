using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    [Header("Transition UI")]

    public CanvasGroup transitionPanelGroup;
    public float fadeDuration;

    public Image loadingIcon;
    public Sprite[] framesLoadingIcon;
    public float iconAnimSpeed;
    bool animIcon;

    public void LoadScene(int sceneIndex, bool pauseButton)
    {
            StartCoroutine(SceneTransition(sceneIndex, pauseButton));
    }

    public IEnumerator SceneTransition(int sceneToLoadBuildIndex, bool pauseButton)
    {
        float fadeTimer = 0f;

        animIcon = true;
        StartCoroutine(LoadIconAnim());

        transitionPanelGroup.interactable = true;
        transitionPanelGroup.blocksRaycasts = true;

        while (fadeTimer < fadeDuration)
        {
            transitionPanelGroup.alpha = (fadeTimer / fadeDuration);
            yield return null;
            fadeTimer += Time.deltaTime;             
        }
        
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneToLoadBuildIndex, LoadSceneMode.Single);
        load.allowSceneActivation = false;
        GameManager.Instance.pauseHandler.PauseButtonDisplay(pauseButton);
        
        yield return new WaitUntil(()=> load.progress == 0.9f);

        GameManager.Instance.inputManager.canMoveCam = true;
        GameManager.Instance.inputManager.canZoomCam = true;
        GameManager.Instance.inputManager.canUseCam = true;

        if (sceneToLoadBuildIndex == GameManager.Instance.pauseHandler.menuScene)
            GameManager.Instance.pauseHandler.pauseButton.SetActive(false);

        load.allowSceneActivation = true;

        yield return new WaitForSeconds(2f);

        while (fadeTimer > 0)
        {
            transitionPanelGroup.alpha = (fadeTimer / fadeDuration);
            yield return null;
            fadeTimer -= Time.deltaTime;
        }

        transitionPanelGroup.interactable = false;
        transitionPanelGroup.blocksRaycasts = false;
        animIcon = false;
    }

    public IEnumerator LoadIconAnim()
    {
        loadingIcon.gameObject.SetActive(true);

        while (animIcon)
        {
            for (int i = 0; i < framesLoadingIcon.Length; i++)
            {
                loadingIcon.sprite = framesLoadingIcon[i];
                yield return new WaitForSecondsRealtime(iconAnimSpeed / framesLoadingIcon.Length);
            }
        }

        loadingIcon.gameObject.SetActive(false);
    }
}
