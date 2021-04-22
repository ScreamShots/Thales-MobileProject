using UnityEngine;
using NaughtyAttributes;

[ExecuteInEditMode]
public class GlobeController : MonoBehaviour
{
    [Header("Globe Component")]
    public Transform anchor;

    [Header("Globe Parameter")]
    public Vector2 aimPos;

    public Vector2 goToPos = Vector2.zero;
    public bool inRoad = false;
    [Button("Go go go!")]
    public void GoGoGo()
    {
        goToPos.x = goToPos.x % 360;
        goToPos.y = Mathf.Clamp(goToPos.y, -90, 90);

        inRoad = true;
    }

    [ReadOnly] public Quaternion aimRot = Quaternion.identity;
    [ReadOnly] public Vector3 pointOnSphere = Vector3.zero;
    [Range(0.5f, 2f)] public float sphereRadius = 1f;

    [Space(5)]
    [HideInInspector] public float xAngleEvol;
    [HideInInspector] public float yAngleEvol;

    [Space(5)]
    [Range(1,10)]public float aimSpeed = 1f;

    [Button("Calcul Rotation")]
    public void CalculateRotation()
    {
        //Clean Value
        aimPos.x = aimPos.x % 360;
        aimPos.y = Mathf.Clamp(aimPos.y, -90, 90);

        //Matrix opération #CounterGimbleLock
        Vector3 sens = new Vector3(0, 0, sphereRadius);
        Matrix4x4 tranforMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-aimPos.y, aimPos.x, 0), Vector3.one);
        pointOnSphere = tranforMatrix.MultiplyPoint(sens);

        //Calcul Rotation
        aimRot.SetLookRotation(pointOnSphere, Vector3.up);

        //Set Globe Rotation
        anchor.rotation = aimRot;
    }

    private void Update()
    {
        if (inRoad)
        {
            aimPos = Vector2.Lerp(aimPos, goToPos, 0.1f);

            if((goToPos - aimPos).magnitude < 10)
            {
                aimPos = goToPos;
                inRoad = false;
            }
        }

        CalculateRotation();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(Vector3.zero, -pointOnSphere, Color.red);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(-pointOnSphere, 0.1f);
    }
}
