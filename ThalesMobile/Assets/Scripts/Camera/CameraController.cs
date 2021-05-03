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
    [Range(1, 10)] public float zoomSpeed = 5;
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
    public Vector2 moveDirection = Vector2.zero;

    [Header("Map Limits")]
    public Boundary limit = new Boundary();

    [Header("Debug")]
    public bool standAloneMode = false;
    [Space(10)]
    public bool drawLine = true;
    public Color edgeColor = Color.red;
    [Range(0.1f, 10f)] public float debugSize = 0.1f;
    public int debugStep = 4;
    [Space(5)]
    public bool drawFocusPoint = false;


    void Start()
    {
        lookAtTraget = false;
        cam = Camera.main;

        if (!standAloneMode)
        {
            GameManager.Instance.cameraController = this;
            GameManager.Instance.inputManager.mainCamera = cam;
            GameManager.Instance.inputManager.camController = this;
        }

        InitializeFocusPoint();

        StartPos();
    }
    void Update()
    {
        if (standAloneMode)
        {
            moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        if (lookAtTraget)
        {
            FocusOnTarget();
        }
        else
        {
            MoveFocusPoint(moveDirection);
        }

        ZoomCalcul(zoomIntensity);
        ZoomApplication();
    }

    [ContextMenu("Set Cam at Start")]
    public void StartPos()
    {
        ZoomCalcul(zoomIntensity);

        //Pos
        aimPos = focusPoint.position + new Vector3(0, aimHeight, aimPromimity);
        transform.position = aimPos;
        //FOV
        cam.fieldOfView = aimFov;
        //Rotation
        transform.rotation = Quaternion.Euler(aimAngle, transform.rotation.y, transform.rotation.z);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        lookAtTraget = true;
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
    
    //Mouvement
    private void MoveFocusPoint(Vector2 dir)
    {
        dir = dir.normalized;

        Vector3 mouvement = new Vector3(dir.x, 0, dir.y) * moveSpeed * Time.deltaTime;
        Vector3 wantedPos = focusPoint.position + mouvement;

        if (!limit.InBoundary(wantedPos))
        {
            wantedPos.x = Mathf.Clamp(wantedPos.x, limit.leftBorder, limit.rightBorder);
            wantedPos.z = Mathf.Clamp(wantedPos.z, limit.downBorder, limit.upBorder);
        }

        focusPoint.position = wantedPos;
    }
    private void FocusOnTarget()
    {
        Vector3 toTarget = (target.position - focusPoint.position);
        if (toTarget.magnitude > (toTarget.normalized).magnitude * refocusSpeed * Time.deltaTime)
        {
            toTarget = toTarget.normalized * refocusSpeed * Time.deltaTime;
        }
        else
        {
            lookAtTraget = false;
        }

        Vector3 wantedPos = focusPoint.position + toTarget;

        focusPoint.position = new Vector3(wantedPos.x, 0 , wantedPos.z);
    }

    //Debug
    private void OnDrawGizmos()
    {
        DebugBoundary(debugSize, debugStep);
    }
    private void DebugBoundary(float height, int step)
    {
        float ratio = height / step;

        if (drawLine)
        {
            //Draw the rectangle
            for (float tempHeight = 0; tempHeight <= height; tempHeight += ratio)
            {
                // _  
                Vector3 drawPos = new Vector3(limit.leftBorder, tempHeight, limit.upBorder);
                Debug.DrawRay(drawPos, Vector3.right * limit.size.x, edgeColor);
                // _
                //  |
                drawPos += Vector3.right * limit.size.x;
                Debug.DrawRay(drawPos, Vector3.back * limit.size.y, edgeColor);
                // _
                // _|
                drawPos += Vector3.back * limit.size.y;
                Debug.DrawRay(drawPos, Vector3.left * limit.size.x, edgeColor);
                // _
                //|_|
                drawPos += Vector3.left * limit.size.x;
                Debug.DrawRay(drawPos, Vector3.forward * limit.size.y, edgeColor);
            }

            Gizmos.color = edgeColor;
            Gizmos.DrawWireCube(new Vector3(limit.offSet.x, height * 0.5f, limit.offSet.y), new Vector3(limit.size.x, height, limit.size.y));

            Gizmos.color = Color.white;
        }

        if (focusPoint != null && drawFocusPoint)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector3(focusPoint.position.x, height, focusPoint.position.z), 0.5f);
            Gizmos.color = Color.white;
        }
    }
}