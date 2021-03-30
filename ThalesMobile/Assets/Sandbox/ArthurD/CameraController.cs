using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform focusPoint = null;
    public Camera cam = null;

    public Transform target = null;
    public bool lookAtTraget = false;

    [Header("Zoom Parameter")]
    [Range(0, 1)] public float zoomIntensity;

    public float minAngle = 30f;
    public float maxAngle = 90f;

    public float minHeight = 3f;
    public float maxHeight = 20f;

    public float minTargetProximity = -5f;
    public float maxTargetProximity = 0f;

    public float minFov = 60f;
    public float maxFov = 110f;

    public float aimLerpSpeed = 0.05f;

    float aimHeight;
    float aimPromimity;

    [Header("Move Parameter")]
    [SerializeField] Vector2 debugDir = Vector2.zero;
    public float moveSpeed = 10f;

    public Vector2 boundary = new Vector2(8, 10);
    public Vector2 offSet = new Vector2(0, 0);

    public float mouvLerpSpeed = 0.1f;

    void Start()
    {
        //Create the focus point
        if(focusPoint == null)
        {
            focusPoint = Instantiate(new GameObject(), new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity, transform).transform;
        }

        cam = Camera.main;
    }

    void Update()
    {
        debugDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (lookAtTraget)
        {
            Targeting(target);
        }
        else
        {
            MoveFocusPoint(debugDir);
        }

        Zoom(zoomIntensity);
        FollowFocusPoint();
    }

    private void OnDrawGizmos()
    {
        DebugBoundary();
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void Targeting(Transform target)
    {
        Vector3 toTarget = target.position - focusPoint.position;

        Vector3 wantedPos = focusPoint.position + toTarget;
        focusPoint.position = Vector3.Lerp(focusPoint.position, wantedPos, mouvLerpSpeed);
    }

    private void Zoom(float zoom)
    {
        zoom = Mathf.Clamp01(zoom);

        float aimAngle = Mathf.Lerp(minAngle, maxAngle, zoom);
        aimHeight = Mathf.Lerp(minHeight, maxHeight, zoom);
        aimPromimity = Mathf.Lerp(minTargetProximity, maxTargetProximity, zoom);
        float aimFov = Mathf.Lerp(minFov, maxFov, zoom);

        /*Déplacement
        Vector3 wantedPos = focusPoint.position + new Vector3(0, aimHeight, aimPromimity);
        transform.position = Vector3.Lerp(transform.position, wantedPos, aimLerpSpeed);
        */

        //Fov
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,aimFov, aimLerpSpeed);
        
        //Rotation 
        Quaternion aimLook = Quaternion.Euler(aimAngle, transform.rotation.y, transform.rotation.z);
        //Slerp and not Lerp (https://youtu.be/uNHIPVOnt-Y)
        transform.rotation = Quaternion.Slerp(transform.rotation, aimLook, aimLerpSpeed);
    }

    private void MoveFocusPoint(Vector2 dir)
    {
        dir = dir.normalized;

        Vector3 mouvement = new Vector3(dir.x, 0, dir.y) * moveSpeed * Time.deltaTime;

        Vector3 wantedPos = focusPoint.position + mouvement;

        if (!InBoundary(wantedPos))
        {
            /*
            wantedPos = focusPoint.position;
            Vector3 newMouv = Vector3.zero;

            //Do max 10 iteration to find the closest point
            while (InBoundary(wantedPos))
            {
                newMouv += mouvement.normalized * (0.1f * mouvement.magnitude);
                wantedPos = focusPoint.position + newMouv;
            }
            newMouv -= mouvement.normalized * (0.1f * mouvement.magnitude);
            */

            wantedPos.x = Mathf.Clamp(wantedPos.x, -(boundary.x * 0.5f) + offSet.x, +(boundary.x * 0.5f) + offSet.x);
            wantedPos.z = Mathf.Clamp(wantedPos.z, -(boundary.y * 0.5f) + offSet.y, +(boundary.y * 0.5f) + offSet.y);

        }

        focusPoint.position = wantedPos;
    }

    private void FollowFocusPoint()
    {
        Vector3 wantedPos = focusPoint.position + new Vector3(0, aimHeight, aimPromimity);
        transform.position = Vector3.Lerp(transform.position, wantedPos, mouvLerpSpeed);
    }

    private bool InBoundary (Vector3 pos)
    {
        //Est ce que je dépasse en x ?
        if(pos.x < -(boundary.x * 0.5f) + offSet.x || boundary.x * 0.5f + offSet.x < pos.x)
        {
            return false;
        }
        else
        //Est ce que je dépasse en y ?
        if (pos.z < -(boundary.y * 0.5f) + offSet.y || boundary.y * 0.5f + offSet.y < pos.z)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void DebugBoundary()
    {
        float height = 2f;

        for (float tempHeight = 0; tempHeight <= height; tempHeight += 0.5f)
        {
            //Top Left
            Vector3 drawPos = new Vector3(-(boundary.x * 0.5f) + offSet.x, tempHeight, +(boundary.y * 0.5f) + offSet.y);
            Debug.DrawRay(drawPos, Vector3.right * boundary.x, Color.red);

            //TopRight
            drawPos += Vector3.right * boundary.x;
            Debug.DrawRay(drawPos, Vector3.back * boundary.y, Color.red);

            //BotoomRight
            drawPos += Vector3.back * boundary.y;
            Debug.DrawRay(drawPos, Vector3.left * boundary.x, Color.red);

            //BotoomLeft
            drawPos += Vector3.left * boundary.x;
            Debug.DrawRay(drawPos, Vector3.forward * boundary.y, Color.red);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(offSet.x, height * 0.5f, offSet.y), new Vector3(boundary.x, height, boundary.y));

        if(focusPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector3(focusPoint.position.x, height, focusPoint.position.z), 0.5f);
        }
    }

}
