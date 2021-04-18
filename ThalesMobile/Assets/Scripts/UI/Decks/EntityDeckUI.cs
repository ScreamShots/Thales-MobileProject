using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDeckUI : MonoBehaviour
{
    //Manager
    [HideInInspector]public UIHandler handler;
    private List<GameObject> decks = new List<GameObject>();
    

    [Header("UI Elements")]
    public GameObject currentDeck;
    public GameObject container;
   
    public void Initialize()
    {
        for (int i = 0; i < handler.entities.Count; i++)
        {
            GameObject temp = Instantiate(handler.entities[i].entityDeck, container.transform);
            temp.SetActive(false);
            handler.entities[i].entityDeck = temp;
            decks.Add(temp);
            handler.uIElements.Add(temp);
        }
    }

    public void UpdateCurrentDeck(GameObject newDeck)
    {
        if(GameManager.Instance.inputManager.currentSelectedCard != null)
            GameManager.Instance.inputManager.currentSelectedCard.abortHandler();
        
        if(currentDeck !=null)
            currentDeck.SetActive(false);
        
        currentDeck = newDeck;
        currentDeck.SetActive(true);


        //Animate Deck Transition
    }

}
