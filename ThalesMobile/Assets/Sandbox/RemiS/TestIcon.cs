using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIcon : MonoBehaviour
{
    RectTransform iconRectTranform;

    TweeningAnimator appearAnim;
    TweeningAnimator moveAnim;

    bool isActive;

    private void Start()
    {
        appearAnim.GetCanvasGroup();
        moveAnim.GetCanvasGroup();
        iconRectTranform = gameObject.GetComponent<RectTransform>();
    }
}
