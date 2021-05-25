using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseHandler : MonoBehaviour
{
    public bool pause;
    public GameObject pauseButton;
    public GameObject pausePanel;
    private SceneHandler sceneHandler;

    [Header("Sound")]

    [SerializeField]
    AudioMixerGroup targetGroup;
    [SerializeField]
    AudioClip gearClick;
    [SerializeField]
    float gearClickSoundVolume;
    [SerializeField]
    AudioClip selectClick;
    [SerializeField]
    float selectSoundVolume;
    [SerializeField]
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        sceneHandler = GameManager.Instance.sceneHandler;
        source.ignoreListenerPause = true;
    }

    public void Pause()
    {
        if(!pause)
        {
            source.volume = Mathf.Clamp01(gearClickSoundVolume);
            GameManager.Instance.soundHandler.PlaySound(gearClick, source, targetGroup);
            AudioListener.pause = true;
            GameManager.Instance.currentGameState = GameManager.GameStates.Pause;
            pause = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            StartCoroutine(PauseUpdate());
        }
        else
        {
            source.volume = Mathf.Clamp01(selectSoundVolume);
            GameManager.Instance.soundHandler.PlaySound(selectClick, source, targetGroup);
            AudioListener.pause = false;
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
        source.volume = Mathf.Clamp01(selectSoundVolume);
        GameManager.Instance.soundHandler.PlaySound(selectClick, source, targetGroup);
        Pause();
        sceneHandler.LoadScene(0, false);
    }

    public void PauseButtonDisplay(bool state)
    {
        pauseButton.SetActive(state);
    }
}
