using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using UnityEngine.UI;

public class EntitiesSelectionUI : MonoBehaviour
{

    //Manager
    [HideInInspector]public UIHandler handler;
    [HideInInspector]public List<EntitiesSelectionButton> buttons = new List<EntitiesSelectionButton>(); 
    
    [Header("UI Elements")]
    public GameObject entitySelectionParent;
    public GameObject entitySelectionForeground;

    public GameObject helicopterSelectionParent;
    public GameObject helicopterSelectionForeground;

    [Header("Buttons References")]
    public GameObject shipButton;
    public GameObject planeButton;
    public GameObject helicopterButton;

    [HideInInspector] public EntitiesSelectionButton currentButton;


    // Start is called before the first frame update
    void Start()
    {
        handler.uIElements.Add(entitySelectionForeground);
        handler.uIElements.Add(entitySelectionParent);

        handler.uIElements.Add(helicopterSelectionForeground);
        handler.uIElements.Add(helicopterSelectionParent);
    }

    public void UpdateButtons(bool active)
    {
        if (!active)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].button.interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].button.interactable = true;
            }
        }
    }

    public void Initialize()
    {
        for (int i = 0; i < handler.entities.Count; i++)
        {
            if(handler.entities[i].GetType() == typeof(OceanEntities.Plane))
            {
                GameObject temp =  Instantiate(planeButton, entitySelectionParent.transform);
                var esb = temp.GetComponent<EntitiesSelectionButton>();
                esb.Initalize();
                esb.linkedEntity = handler.entities[i];
                esb.manager = this;
                temp.transform.localScale = Vector3.one;
                buttons.Add(esb);
                handler.uIElements.Add(temp);
                handler.entities[i].linkedButton = esb;
            }
            else if(handler.entities[i].GetType() == typeof(Ship))
            {
                GameObject temp = Instantiate(shipButton, entitySelectionParent.transform); ;
                var esb = temp.GetComponent<EntitiesSelectionButton>();
                esb.Initalize();
                esb.linkedEntity = handler.entities[i];
                esb.manager = this;
                temp.transform.localScale = Vector3.one;
                buttons.Add(esb);
                handler.uIElements.Add(temp);
                handler.entities[i].linkedButton = esb;
            }
            else if (handler.entities[i].GetType() == typeof(Helicopter))
            {
                GameObject temp = Instantiate(helicopterButton, helicopterSelectionParent.transform); ;
                var esb = temp.GetComponent<EntitiesSelectionButton>();
                esb.Initalize();
                esb.linkedEntity = handler.entities[i];
                esb.manager = this;
                temp.transform.localScale = Vector3.one;
                buttons.Add(esb);
                handler.uIElements.Add(temp);
                handler.entities[i].linkedButton = esb;
            }
        }
        entitySelectionForeground.transform.SetAsLastSibling();
        helicopterSelectionForeground.transform.SetAsLastSibling();

        if(buttons.Count > 0)
        {
            buttons[0].SelectEntity();
        }
    }
}
