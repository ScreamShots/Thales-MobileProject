using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameStates {Default, Loading, Playing, Pause}
    public GameStates currentGameState;

    [Header("Input Management")]
    public InputManager inputManager;
    public PlayerController playerController;

    [Header("Camera")]
    public CameraController cameraController;

    [Header("Level")]
    public LevelManager levelManager;

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

    public Coroutine ExternalStartCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void ExternalStopCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }
}
