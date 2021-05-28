using System.Collections;
using UnityEngine;
using Tweek.FlagAttributes;
using NaughtyAttributes;

[TweekClass]
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
    [TweekFlag(FieldUsage.Gameplay)] [Range(1, 10)] public float zoomSpeed = 5;
    [TweekFlag(FieldUsage.Gameplay), Expandable] public CameraSettings camSett;
    private float aimAngle;
    private float aimHeight;
    private float aimPromimity;
    private float aimFov;
    [SerializeField] float moveZoomLvlFactor = 1f;
    [TweekFlag(FieldUsage.Gameplay), SerializeField, Range(0, 1)] float minimalMoveFactor = 0.5f;
    [Space(10)]
    [TweekFlag(FieldUsage.Gameplay)] public float aimLerpSpeed = 0.05f;

    [Header("Move Parameter")]
    [TweekFlag(FieldUsage.Gameplay)] public float moveSpeed = 10f;
    [TweekFlag(FieldUsage.Gameplay)] public float refocusSpeed = 100f;
    [Space(10)]
    [TweekFlag(FieldUsage.Gameplay)] public float mouvLerpSpeed = 0.1f;
    private Vector3 aimPos = Vector3.zero;
    [Space(5)]
    public Vector2 moveDirection = Vector2.zero;

    [Header("Map Limits")]
    public Boundary limit = new Boundary();
    public Boundary limitDezoom = new Boundary();

    [Header("Debug")]
    public bool standAloneMode = false;
    [Space(10)]
    public bool drawLine = true;
    public Color edgeColor = Color.red;
    [Range(0.1f, 10f)] public float debugSize = 0.1f;
    public int debugStep = 4;
    [Space(5)]
    public bool drawFocusPoint = false;


    private void Awake()
    {
        GameManager.Instance.cameraController = this;
    }

    void Start()
    {
        lookAtTraget = false;
        cam = Camera.main;

        if (!standAloneMode)
        {
            GameManager.Instance.cameraController = this;
            GameManager.Instance.inputManager.mainCamera = cam;
            GameManager.Instance.inputManager.camController = this;

            GameManager.Instance.inputManager.maxX = limit.rightBorder;
            GameManager.Instance.inputManager.minX = limit.leftBorder;
            GameManager.Instance.inputManager.maxY = limit.upBorder;
            GameManager.Instance.inputManager.minY = limit.downBorder;

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
            //Go to the focus Point
            FocusOnTarget();
        }
        else
        {
            //Recive Moving Input
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
        zoomIntensity = value;
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

        //Move Factor 
        float mapScaleFactor = Mathf.Min(limitDezoom.size.x / limit.size.x, limitDezoom.size.y / limit.size.y);
        moveZoomLvlFactor = Mathf.Lerp(1, mapScaleFactor, zoomIntensity) * (1 - minimalMoveFactor) + minimalMoveFactor;
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

        Vector3 mouvement = new Vector3(dir.x, 0, dir.y) * moveSpeed * moveZoomLvlFactor * Time.deltaTime;
        Vector3 wantedPos = focusPoint.position + mouvement;

        wantedPos = ClampInCamZone(wantedPos);

        focusPoint.position = wantedPos;
    }
    private void FocusOnTarget()
    {

        Vector3 toTarget = (target.position - focusPoint.position);

        if (!limitDezoom.InBoundary(target.position))
        {
            float aimZoomLvlY = 1;
            float aimZoomLvlX = 1;
            //Top
            if (target.position.y > limitDezoom.upBorder)
            {
                aimZoomLvlY = Mathf.InverseLerp(limit.upBorder, limitDezoom.upBorder, target.position.z);
            }
            //Bottom
            else if (target.position.y < limitDezoom.downBorder)
            {
                aimZoomLvlY = Mathf.InverseLerp(limit.downBorder, limitDezoom.downBorder, target.position.z);
            }
            //Left
            if (target.position.x < limitDezoom.leftBorder)
            {
                aimZoomLvlX = Mathf.InverseLerp(limit.leftBorder, limitDezoom.leftBorder, target.position.x);
            }
            //right
            else if (limitDezoom.rightBorder < target.position.x)
            {
                aimZoomLvlX = Mathf.InverseLerp(limit.rightBorder, limitDezoom.rightBorder, target.position.x);
            }

            SetZoom(Mathf.Min(aimZoomLvlX, aimZoomLvlY), 2);
        }


        if (toTarget.magnitude > (toTarget.normalized).magnitude * refocusSpeed * Time.deltaTime)
        {
            toTarget = toTarget.normalized * refocusSpeed * moveZoomLvlFactor * Time.deltaTime;
        }
        else
        {
            lookAtTraget = false;
            toTarget = target.position - focusPoint.position;
            //Sécurité en cas de zoom infini
            StopAllCoroutines();
        }

        Vector3 wantedPos = focusPoint.position + toTarget;

        wantedPos = ClampInCamZone(wantedPos);

        focusPoint.position = new Vector3(wantedPos.x, 0, wantedPos.z);
    }

    private Vector3 ClampInCamZone(Vector3 pos)
    {
        Vector3 result = pos;

        result.x = Mathf.Clamp(pos.x, Mathf.Lerp(limit.leftBorder, limitDezoom.leftBorder, zoomIntensity), Mathf.Lerp(limit.rightBorder, limitDezoom.rightBorder, zoomIntensity));
        result.z = Mathf.Clamp(pos.z, Mathf.Lerp(limit.downBorder, limitDezoom.downBorder, zoomIntensity), Mathf.Lerp(limit.upBorder, limitDezoom.upBorder, zoomIntensity));

        return result;
    }

    #region Debug
    private void OnDrawGizmos()
    {
        //DebugBoundary(debugSize, debugStep, limit);
        //DebugBoundary(debugSize, debugStep, limitDezoom);

        DebugCamZone(limit, limitDezoom, debugStep);

        //Draw Focus Point
        if (focusPoint != null && drawFocusPoint)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector3(focusPoint.position.x, 0, focusPoint.position.z), 0.5f);
            Gizmos.color = Color.white;
        }
    }
    private void DebugBoundary(float height, int step, Boundary limit)
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
    private void DebugCamZone(Boundary limit, Boundary limitDezoom, int step)
    {
        float height = camSett.maxHeight - camSett.minHeight;
        float ratio = height / step;

        if (drawLine)
        {
            Boundary heightBound = limit;

            float i = 0;
            for (float tempHeight = camSett.minHeight; tempHeight <= camSett.maxHeight; tempHeight += ratio)
            {
                heightBound.size = new Vector2(
                    Mathf.Lerp(limit.size.x, limitDezoom.size.x, i),
                    Mathf.Lerp(limit.size.y, limitDezoom.size.y, i));

                heightBound.offSet = new Vector2(
                    Mathf.Lerp(limit.offSet.x, limitDezoom.offSet.x, i),
                    Mathf.Lerp(limit.offSet.y, limitDezoom.offSet.y, i));

                heightBound.offSet.y += Mathf.Lerp(camSett.minTargetProximity, camSett.maxTargetProximity, i);

                i += (float)1f / step;

                //Draw the rectangle
                // _  
                Vector3 drawPos = new Vector3(heightBound.leftBorder, tempHeight, heightBound.upBorder);
                Debug.DrawRay(drawPos, Vector3.right * heightBound.size.x, edgeColor);
                // _
                //  |
                drawPos += Vector3.right * heightBound.size.x;
                Debug.DrawRay(drawPos, Vector3.back * heightBound.size.y, edgeColor);
                // _
                // _|
                drawPos += Vector3.back * heightBound.size.y;
                Debug.DrawRay(drawPos, Vector3.left * heightBound.size.x, edgeColor);
                // _
                //|_|
                drawPos += Vector3.left * heightBound.size.x;
                Debug.DrawRay(drawPos, Vector3.forward * heightBound.size.y, edgeColor);
            }

            #region Cone Corners
            Vector3 LeftBottomCornerMin = new Vector3(limit.leftBorder, camSett.minHeight, limit.downBorder + camSett.minTargetProximity);
            Vector3 LeftBottomCornerMax = new Vector3(limitDezoom.leftBorder, camSett.maxHeight, limitDezoom.downBorder + camSett.maxTargetProximity);
            Debug.DrawLine(LeftBottomCornerMin, LeftBottomCornerMax, Color.red);

            Vector3 LeftTopCornerMin = new Vector3(limit.leftBorder, camSett.minHeight, limit.upBorder + camSett.minTargetProximity);
            Vector3 LeftTopCornerMax = new Vector3(limitDezoom.leftBorder, camSett.maxHeight, limitDezoom.upBorder + camSett.maxTargetProximity);
            Debug.DrawLine(LeftTopCornerMin, LeftTopCornerMax, Color.red);

            Vector3 RightBottomCornerMin = new Vector3(limit.rightBorder, camSett.minHeight, limit.downBorder + camSett.minTargetProximity);
            Vector3 RightBottomCornerMax = new Vector3(limitDezoom.rightBorder, camSett.maxHeight, limitDezoom.downBorder + camSett.maxTargetProximity);
            Debug.DrawLine(RightBottomCornerMin, RightBottomCornerMax, Color.red);

            Vector3 RightTopCornerMin = new Vector3(limit.rightBorder, camSett.minHeight, limit.upBorder + camSett.minTargetProximity);
            Vector3 RightTopCornerMax = new Vector3(limitDezoom.rightBorder, camSett.maxHeight, limitDezoom.upBorder + camSett.maxTargetProximity);
            Debug.DrawLine(RightTopCornerMin, RightTopCornerMax, Color.red);
            #endregion

            #region Draw on Sea
            //Draw the rectangle
            // _  
            Vector3 drawPos2 = new Vector3(limit.leftBorder, 0, limit.upBorder);
            Debug.DrawRay(drawPos2, Vector3.right * limit.size.x, edgeColor);
            // _
            //  |
            drawPos2 += Vector3.right * limit.size.x;
            Debug.DrawRay(drawPos2, Vector3.back * limit.size.y, edgeColor);
            // _
            // _|
            drawPos2 += Vector3.back * limit.size.y;
            Debug.DrawRay(drawPos2, Vector3.left * limit.size.x, edgeColor);
            // _
            //|_|
            drawPos2 += Vector3.left * limit.size.x;
            Debug.DrawRay(drawPos2, Vector3.forward * limit.size.y, edgeColor);
            #endregion

        }
    }
    #endregion
}
