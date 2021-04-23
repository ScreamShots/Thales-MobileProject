using UnityEngine;

[ExecuteAlways]
public class ButtonToWorld : MonoBehaviour
{
    public Vector3 aimPos;
    public Camera cam;
    private RectTransform rectTrans;
    private GameObject me;

    private void Awake()
    {
        rectTrans = this.GetComponent<RectTransform>();
        me = this.transform.gameObject;
    }

    private void Update()
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(aimPos);
        rectTrans.position = screenPoint;

        float dot;
        dot = Vector3.Dot(cam.transform.forward, aimPos);

        me.SetActive(dot >= 0);
    }
}
