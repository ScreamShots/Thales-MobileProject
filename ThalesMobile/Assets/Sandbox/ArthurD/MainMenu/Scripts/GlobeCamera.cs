using UnityEngine;

public class GlobeCamera : MonoBehaviour
{
    [Header("Cam Parameter")]
    public Camera cam;
    public Transform anchorPos;

    [Header("Globe Parameter")]
    public Transform target;
    public float distToGlobe = 4f;
    public float aimLerpSpeed = 0.1f;

    public Vector2 aimPos;

    private float xAngleEvol;
    private float yAngleEvol;

    private void Update()
    {
        //keep on globe
        anchorPos.position = target.position;

        //Distance Set-up
        transform.localPosition = new Vector3(target.transform.localScale.x + distToGlobe, 0, 0);

        //Calcul next pos
        xAngleEvol = Mathf.Lerp(anchorPos.transform.eulerAngles.y, aimPos.x, aimLerpSpeed);
        yAngleEvol = Mathf.Lerp(anchorPos.transform.eulerAngles.z, aimPos.y, aimLerpSpeed);

        //Set next pos
        anchorPos.eulerAngles = new Vector3( 0, xAngleEvol, yAngleEvol);
    }
}
