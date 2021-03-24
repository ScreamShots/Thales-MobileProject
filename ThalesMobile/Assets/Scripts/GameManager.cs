using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameStates {Default, Loading, Playing, Pause}
    public GameStates currentGameState;

    private void Awake()
    {
        #region Singleton Declaration / DDOL
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }
}
