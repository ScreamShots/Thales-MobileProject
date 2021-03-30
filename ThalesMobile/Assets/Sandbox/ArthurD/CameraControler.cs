using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform target = null;
    public bool lookAtTraget = false;

    [Header("Zoom Parameter")]
    [Range(0,1)] public float zoomIntensity;

    Vector3 focusPos = new Vector3(0, 10, 0);
    public float handModeHeight = 10f;

    public float minAngle = 30f;
    public float maxAngle = 90f; 

    public float minHeight = 3f;
    public float maxHeight = 20f;

    public float minTargetProximity = 3f;
    public float maxTargetProximity = 0f;

    public float aimLerpSpeed = 0.05f;

    [Header("Move Parameter")]
    [SerializeField] Vector2 tempDir = Vector2.zero;
    public float moveSpeed = 10f;

    public Vector2 boundary = new Vector2(8, 10);
    public Vector2 offSet = new Vector2(0, 0);

    public float mouvLerpSpeed = 0.1f;

    private void Start()
    {
        focusPos = new Vector3(transform.position.x, handModeHeight, transform.position.z);
    }

    void Update()
    {
        if (lookAtTraget)
        {
            ZoomOn(zoomIntensity, target);
            Follow(target);

            focusPos = new Vector3(transform.position.x, handModeHeight, transform.position.z);
        }
        else
        {
            Zoom(zoomIntensity);
            Move(tempDir);
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Zoom(float zoom)
    {
        zoom = Mathf.Clamp01(zoom);

        float aimAngle = Mathf.Lerp(minAngle, maxAngle, zoom);
        float aimHeight = Mathf.Lerp(minHeight, maxHeight, zoom);
        float aimPromimity = Mathf.Lerp(minTargetProximity, maxTargetProximity, zoom);


        Vector3 wantedPos = focusPos + new Vector3(0, aimHeight, aimPromimity);
        transform.position = Vector3.Lerp(transform.position, wantedPos, aimLerpSpeed);

        //Rotation 
        Quaternion aimLook = Quaternion.Euler(aimAngle, transform.rotation.y, transform.rotation.z);
        //Slerp and not Lerp
        transform.rotation = Quaternion.Slerp(transform.rotation, aimLook, aimLerpSpeed);

    }
    private void ZoomOn(float zoom, Transform target)
    {
        zoom = Mathf.Clamp01(zoom);

        float aimAngle = Mathf.Lerp(minAngle, maxAngle, zoom);
        float aimHeight = Mathf.Lerp(minHeight, maxHeight, zoom);
        float aimPromimity = Mathf.Lerp(minTargetProximity, maxTargetProximity, zoom);


        Vector3 wantedPos = target.position + new Vector3(0, aimHeight, aimPromimity);
        transform.position = Vector3.Lerp(transform.position, wantedPos, aimLerpSpeed);

        //Aim target directly 
        //Quaternion aimLook = Quaternion.LookRotation(target.position - transform.position);
        Quaternion aimLook = Quaternion.Euler(aimAngle, transform.rotation.y, transform.rotation.z);

        //Slerp and not Lerp
        transform.rotation = Quaternion.Slerp(transform.rotation, aimLook, aimLerpSpeed);

    }

    private void Move(Vector2 dir)
    {
        dir = dir.normalized;
        Vector3 mouvement = new Vector3(dir.x, 0, dir.y) * moveSpeed;

        Vector3 wantedPos = target.position + mouvement;

        wantedPos.x = Mathf.Clamp(wantedPos.x, -(boundary.x * 0.5f) + offSet.x, +(boundary.x * 0.5f) + offSet.x);
        wantedPos.z = Mathf.Clamp(wantedPos.z, -(boundary.y * 0.5f) + offSet.y, +(boundary.y * 0.5f) + offSet.y);

        transform.position = Vector3.Lerp(transform.position, wantedPos, mouvLerpSpeed);
    }
    private void Follow(Transform target)
    {
        Vector2 toTarget = new Vector2(target.position.x, target.position.y) - new Vector2(transform.position.x, transform.position.y);

        Vector3 wantedPos = target.position + new Vector3(toTarget.x, 0, toTarget.y);
        wantedPos.x = Mathf.Clamp(wantedPos.x, -(boundary.x * 0.5f) + offSet.x, +(boundary.x * 0.5f) + offSet.x);
        wantedPos.z = Mathf.Clamp(wantedPos.z, -(boundary.y * 0.5f) + offSet.y, +(boundary.y * 0.5f) + offSet.y);

        transform.position = Vector3.Lerp(transform.position, wantedPos, mouvLerpSpeed);
    }

    private void OnDrawGizmos()
    {
        DebugBoundary();
    }

    private void DebugBoundary()
    {
        float height = 4f;
        //Top Left
        Vector3 drawPos = new Vector3(-(boundary.x * 0.5f) + offSet.x, height, +(boundary.y * 0.5f) + offSet.y);
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

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, height, transform.position.z), 1f);
    }
}
