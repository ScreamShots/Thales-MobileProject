using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using UnityEngine.UI;

public class EntitiesSelectionUI : MonoBehaviour
{

    LevelManager levelManager;
    List<PlayerOceanEntity> entities = new List<PlayerOceanEntity>();
    
    [Header("UI Elements")]
    public GameObject selectionParent;
    public GameObject entitySelectionForeground;

    [Header("Buttons References")]
    public GameObject shipButton;
    public GameObject planeButton;
    public GameObject helicopterButton;

    [Header("Entity Sprites")]
    public Sprite shipSprite;
    public Sprite planeSprite;
    public Sprite helicopterSprite;

    [HideInInspector] public EntitiesSelectionButton currentButton;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        levelManager = GameManager.Instance.levelManager;
        entities = levelManager.playerOceanEntities;

        Initialize();

        entitySelectionForeground.transform.SetAsLastSibling();
    }

    private void Initialize()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            if(entities[i].GetType() !=  typeof(Helicopter))
            {
                if(entities[i].GetType() == typeof(OceanEntities.Plane))
                {
                    GameObject temp =  Instantiate(planeButton, selectionParent.transform);
                    var esb = temp.GetComponent<EntitiesSelectionButton>();
                    esb.linkedEntity = entities[i];
                    esb.manager = this;
                    temp.transform.localScale = Vector3.one;
                }
                else if(entities[i].GetType() == typeof(Ship))
                {
                    GameObject temp = Instantiate(shipButton, selectionParent.transform);
                    var esb = temp.GetComponent<EntitiesSelectionButton>();
                    esb.linkedEntity = entities[i];
                    esb.manager = this;
                    temp.transform.localScale = Vector3.one;
                }
            }
        }
    }
}
