using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScrollbarControl : MonoBehaviour
{
    public Scrollbar scroll;
    public float size;

    private void Awake()
    {
        scroll = transform.GetComponent<Scrollbar>();
    }

    private IEnumerator Start()
    {
        yield return null;
        scroll.size = size;
    }

    [ContextMenu("La Size!!!")]
    private void OnEnable()
    {
        scroll.size = size;
    }

}
