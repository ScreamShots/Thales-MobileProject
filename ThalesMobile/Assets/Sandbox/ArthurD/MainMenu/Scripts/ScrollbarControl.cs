using UnityEngine;
using UnityEngine.UI;

public class ScrollbarControl : MonoBehaviour
{
    public Scrollbar scroll;
    public float size;

    private void Awake()
    {
        scroll = transform.GetComponent<Scrollbar>();
        scroll.size = size;
    }

    private void Start()
    {
        scroll.size = size;
    }

    [ContextMenu("La Size!!!")]
    private void OnEnable()
    {
        scroll.size = size;
    }

    private void Update()
    {
        scroll.size = size;
    }
}
