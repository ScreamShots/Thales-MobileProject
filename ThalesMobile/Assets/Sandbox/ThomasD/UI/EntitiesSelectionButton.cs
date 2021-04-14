using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using UnityEngine.UI;

public class EntitiesSelectionButton : MonoBehaviour
{
    [HideInInspector]public EntitiesSelectionUI manager;
    [HideInInspector] public PlayerOceanEntity linkedEntity; 
    private Button button;

    public TweeningAnimator animator;


    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectEntity);
    }

    public void SelectEntity()
    {
        if (manager.currentButton != null)
            manager.currentButton.Deselect();
       
        manager.currentButton = this;

        GameManager.Instance.playerController.currentSelectedEntity = linkedEntity;
        GameManager.Instance.cameraController.SetTarget(linkedEntity.transform);

        GameManager.Instance.uiHandler.entityDeckUI.UpdateCurrentDeck(linkedEntity.entityDeck);
        
        //Animate UI button
        StartCoroutine(animator.anim.Play(animator, animator.originalPos));
    }

    public void Deselect()
    {
        //Animate UI button
        StartCoroutine(animator.anim.PlayBackward(animator, animator.originalPos, true));
    }

}
