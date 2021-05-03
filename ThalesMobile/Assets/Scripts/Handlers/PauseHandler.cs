using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public bool pause;
    public GameObject pausePanel;
    private SceneHandler sceneHandler;

    // Start is called before the first frame update
    void Start()
    {
        sceneHandler = GameManager.Instance.sceneHandler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        if(!pause)
        {
            GameManager.Instance.currentGameState = GameManager.GameStates.Pause;
            pause = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            StartCoroutine(PauseUpdate());
        }
        else
        {
            pausePanel.SetActive(false);
            pause = false;
            GameManager.Instance.currentGameState = GameManager.GameStates.Playing;
            Time.timeScale = 1;
        }
    }

    IEnumerator PauseUpdate()
    {
        while(pause)
        {
            //Use this method while on pause;


            yield return null;
        }
    }

    public void ReturnToMenu()
    {
        Pause();
        sceneHandler.LoadScene(0);
    }
}
