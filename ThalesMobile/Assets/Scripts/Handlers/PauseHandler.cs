using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public bool pause;

    // Start is called before the first frame update
    void Start()
    {
        
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
            Time.timeScale = 0;
            StartCoroutine(PauseUpdate());
        }
        else
        {
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
}
