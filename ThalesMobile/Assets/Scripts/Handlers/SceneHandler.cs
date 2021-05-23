using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [Header("Transition UI")]
    public GameObject transitionPanel;
    private Animator transitionPanelAnimator;
    private bool finishedFade = true;

    void Start()
    {
        transitionPanelAnimator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(SceneTransition(2));
        }
    }

    public void LoadScene(int sceneIndex)
    {
        if(finishedFade)
            StartCoroutine(SceneTransition(sceneIndex));
    }

    public IEnumerator SceneTransition(int sceneToLoadBuildIndex)
    {
        //Start Animation and scene load
        transitionPanelAnimator.SetBool("fade", true);
        finishedFade = false;
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneToLoadBuildIndex, LoadSceneMode.Single);
        load.allowSceneActivation = false;
        
        //Wait until scene has finished loading and animation has ended.
        yield return new WaitUntil(()=> load.progress == 0.9f && finishedFade);

        //Activate the scene and unfade.
        load.allowSceneActivation = true;
        transitionPanelAnimator.SetBool("fade", false);
    }

    public void AnimEvent(string paramater)
    {
        if (paramater == "fadeEnded")
        {
            finishedFade = true;
        }    
    }
}
