using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
/// <summary>
/// Antoine Leroux - 31/03/2021 - This script is use to store differents elements from the scene. Need to be a singleton to acess more easily? 
/// </summary>
public class LevelManager : MonoBehaviour
{
    public List<DetectableOceanEntity> submarineEntitiesInScene;

    public List<SonobuoyInstance> sonobuoysInScene;

    public List<DetectionObject> activatedDetectionObjects;

    public List<Submarine> enemyEntitiesInScene;

    public List<PlayerOceanEntity> playerOceanEntities;

    public Environnement environnement;

    private void Start()
    {
        GameManager.Instance.levelManager = this;
    }
}
