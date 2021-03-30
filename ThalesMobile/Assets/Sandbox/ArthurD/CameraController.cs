﻿using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Component")]
    public Transform focusPoint = null;
    public Camera cam = null;
    [Space(10)]
    public bool lookAtTraget = false;
    public Transform target = null;

    [Header("Zoom Parameter")]
    [Range(0, 1)] public float zoomIntensity;
    public CameraSettings camSett = new CameraSettings();
    private float aimAngle;
    private float aimHeight;
    private float aimPromimity;
    private float aimFov;
    [Space(10)]
    public float aimLerpSpeed = 0.05f;

    [Header("Move Parameter")]
    public float moveSpeed = 10f;
    public float refocusSpeed = 100f;
    [Space(10)]
    public float mouvLerpSpeed = 0.1f;
    private Vector3 aimPos = Vector3.zero;
    [Space(5)]
    [SerializeField] bool debug = false;
    [SerializeField] Vector2 debugDir = Vector2.zero;

    [Header("Map Limits")]
    public CameraBoundary limit = new CameraBoundary();

    void Start()
    {
        cam = Camera.main;
        InitializeFocusPoint();
    }
    void Update()
    {
        debugDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (lookAtTraget)
        {
            FocusOnTraget();
        }
        else
        {
            MoveFocusPoint(debugDir);
        }

        ZoomCalcul(zoomIntensity);
        ZoomApplication();
    }
    private void OnDrawGizmos()
    {
        DebugBoundary(4f, 8);
    }

    [ContextMenu("testTo1")]
    public void Test()
    {
        SetZoom(1, 1);
    }
    [ContextMenu("testTo0")]
    public void Test2()
    {
        SetZoom(0, 1);
    }
    public void SetZoom(float zoomdesired, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(ZoomLeveling(zoomdesired, speed));
    }
    private IEnumerator ZoomLeveling(float value, float speed)
    {
        float time = 0f;
        float distance = value - zoomIntensity;
        float distAbsolute = Mathf.Abs(distance);
        float baseZoom = zoomIntensity;

        while (time < distAbsolute)
        {
            time += speed * Time.deltaTime;
            zoomIntensity = Mathf.Lerp(baseZoom, baseZoom + distance, time);
            yield return null;
        }
    }

    public void SetTraget(Transform target)
    {
        this.target = target;
    }
    public void SetIsTargeting(bool value)
    {
        lookAtTraget = value;
    }
    public void ToogleTargeting()
    {
        lookAtTraget = !lookAtTraget;
    }

    private void InitializeFocusPoint()
    {
        if (focusPoint == null)
        {
            focusPoint = Instantiate(new GameObject(), new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity).transform;
            focusPoint.name = "New_CamFocusPoint";
        }
    }
    private void ZoomCalcul(float zoom)
    {
        zoom = Mathf.Clamp01(zoom);
        //Position
        aimHeight = camSett.EvalHeight(zoom);
        aimPromimity = camSett.EvalPromimity(zoom);
        //Fov
        aimFov = camSett.EvalFieldOfView(zoom);
        //Rotation 
        aimAngle = camSett.EvalAngle(zoom);
    }
    private void ZoomApplication()
    {
        //ApplyPos
        aimPos = focusPoint.position + new Vector3(0, aimHeight, aimPromimity);
        transform.position = Vector3.Lerp(transform.position, aimPos, mouvLerpSpeed);

        //Apply Fov
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, aimLerpSpeed);

        //Apply Rotation
        Quaternion aimLook = Quaternion.Euler(aimAngle, transform.rotation.y, transform.rotation.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, aimLook, aimLerpSpeed); //Slerp and not Lerp (https://youtu.be/uNHIPVOnt-Y)
    }
    private void MoveFocusPoint(Vector2 dir)
    {
        dir = dir.normalized;

        Vector3 mouvement = new Vector3(dir.x, 0, dir.y) * moveSpeed * Time.deltaTime;
        Vector3 wantedPos = focusPoint.position + mouvement;

        if (!InBoundary(wantedPos))
        {
            wantedPos.x = Mathf.Clamp(wantedPos.x, limit.left, limit.right);
            wantedPos.z = Mathf.Clamp(wantedPos.z, limit.down, limit.up);
        }

        focusPoint.position = wantedPos;
    }
    private void FocusOnTraget()
    {
        Vector3 toTarget = (target.position - focusPoint.position);
        if (toTarget.magnitude > 0.5f)
        {
            toTarget = toTarget.normalized * refocusSpeed * Time.deltaTime;
        }

        Vector3 wantedPos = focusPoint.position + toTarget;

        focusPoint.position = wantedPos;
    }
    private bool InBoundary(Vector3 pos)
    {
        //Est ce que je dépasse en x ?
        if (pos.x < limit.left || limit.right < pos.x)
        {
            return false;
        }
        else
        //Est ce que je dépasse en y ?
        if (pos.z < limit.down || limit.up < pos.z)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void DebugBoundary(float height, int step)
    {
        float ratio = height / step;

        for (float tempHeight = 0; tempHeight <= height; tempHeight += ratio)
        {
            //Draw the rectangle
            // _  
            Vector3 drawPos = new Vector3(limit.left, tempHeight, limit.up);
            Debug.DrawRay(drawPos, Vector3.right * limit.size.x, Color.red);
            // _
            //  |
            drawPos += Vector3.right * limit.size.x;
            Debug.DrawRay(drawPos, Vector3.back * limit.size.y, Color.red);
            // _
            // _|
            drawPos += Vector3.back * limit.size.y;
            Debug.DrawRay(drawPos, Vector3.left * limit.size.x, Color.red);
            // _
            //|_|
            drawPos += Vector3.left * limit.size.x;
            Debug.DrawRay(drawPos, Vector3.forward * limit.size.y, Color.red);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(limit.offSet.x, height * 0.5f, limit.offSet.y), new Vector3(limit.size.x, height, limit.size.y));

        if (focusPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector3(focusPoint.position.x, height, focusPoint.position.z), 0.5f);
        }
    }
}
