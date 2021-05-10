using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using UnityEngine.UI;

public class EntitiesSelectionButton : MonoBehaviour
{
    [HideInInspector] public EntitiesSelectionUI manager;
    [HideInInspector] public PlayerOceanEntity linkedEntity; 
    [HideInInspector] public Button button;

    public TweeningAnimator animator;
    public TweeningAnimator spotlightAnim;


    // Start is called before the first frame update
    public void Initalize()
    {
        animator.anim = Instantiate(animator.anim);
        spotlightAnim.anim = Instantiate(spotlightAnim.anim);

        spotlightAnim.GetCanvasGroup();

        button = GetComponent<Button>();
        button.onClick.AddListener(SelectEntity);
    }

    public void SelectEntity()
    {
        if (manager.currentButton != null && manager.currentButton != this && manager.currentButton.gameObject.activeInHierarchy)
            manager.currentButton.Deselect();

        if(manager.currentButton != this && gameObject.activeInHierarchy)
        {
            StartCoroutine(animator.anim.Play(animator, animator.originalPos));
            StartCoroutine(spotlightAnim.anim.Play(spotlightAnim, spotlightAnim.originalPos));
        }
        
        manager.currentButton = this;

        GameManager.Instance.playerController.currentSelectedEntity = linkedEntity;


        GameManager.Instance.uiHandler.entityDeckUI.UpdateCurrentDeck(linkedEntity.entityDeck);
            

        if(GameManager.Instance.cameraController != null)
            GameManager.Instance.cameraController.SetTarget(linkedEntity.transform);
    }

    public void Deselect()
    {
        //Animate UI button
        StartCoroutine(animator.anim.PlayBackward(animator, animator.originalPos, true));
        StartCoroutine(spotlightAnim.anim.PlayBackward(spotlightAnim, spotlightAnim.originalPos, true));
    }

}
