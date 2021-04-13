using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
public class UIHandler : MonoBehaviour
{

    [HideInInspector]public LevelManager levelManager;
    [HideInInspector]public List<PlayerOceanEntity> entities = new List<PlayerOceanEntity>();

    [HideInInspector] public List<GameObject> uIElements = new List<GameObject>();

    [Header("Sub-Components")]
    public EntitiesSelectionUI entitiesSelectionUI;
    public EntityDeckUI entityDeckUI;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.uiHandler = this;
        entitiesSelectionUI.handler = this;
        entityDeckUI.handler = this;


        levelManager = GameManager.Instance.levelManager;
        entities = levelManager.playerOceanEntities;

        entityDeckUI.Initialize();
        entitiesSelectionUI.Initialize();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
