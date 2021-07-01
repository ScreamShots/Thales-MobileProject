using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

/// <summary>
/// Antoine Leroux - 28/03/2021 - Hack State is an enum describing the interrest point hacking state.
/// </summary>
public enum HackState
{
    unhacked,
    inHack,
    doneHack
}


/// <summary>
/// Antoine Leroux - 28/03/2021 - InterrestPoint is the class for every interrest point. 
/// </summary>
/// 
 
public class InterestPoint : MonoBehaviour
{
    private Submarine submarine;

    public HackState currentHackState;
    public float hackProgression;

    [Header("IP Settings")]
     
    public float hackTime;
     
    public float hackingRange;

    [Header("IP Alert")]
     
    public bool sendAlert;
     
    public float detectionAlertRange;
     
    public float timeInRangeBeforeAlert;
    private float currentTimeInRange;
    public GameObject rangeDisplay;

    [Header("Feedback")]
    public ParticleSystem hackParticles;
    private GameObject particlesObject;
    public GameObject alertIcon;
    public List<MeshRenderer> meshes;
    public Color hackedMeshColor = Color.white;

    private bool alertFlag;

    private void OnValidate()
    {
        if (sendAlert)
        {
            SetRangeSize();
            rangeDisplay.SetActive(true);
        }
        else
        {
            rangeDisplay.SetActive(false);
        }
    }

    private void Start()
    {
        if (sendAlert)
        {
            submarine = GameManager.Instance.levelManager.enemyEntitiesInScene[0];
        }

        submarine = GameManager.Instance.levelManager.submarine;

        SetRangeSize();
        rangeDisplay.SetActive(false);
        alertIcon.SetActive(false);
        particlesObject = hackParticles.gameObject;
        particlesObject.SetActive(false);

        for (int x = 0; x < meshes.Count; x++)
        {
            meshes[x].material.color = Color.white;
        }
    }

    private void Update()
    {
        if (sendAlert)
        {
            DetectSubmarine();
        }

        if (currentHackState == HackState.doneHack)
        {
            ChangeMeshColor();
            EmitHackParticles(true);
        }
    }

    private void SetRangeSize()
    {
        rangeDisplay.transform.localScale = new Vector2(detectionAlertRange * 2, detectionAlertRange * 2);
    }

    private void ChangeMeshColor()
    {
        for (int x = 0; x < meshes.Count; x++)
        {
            if (meshes[x].material.color != hackedMeshColor)
            {
                meshes[x].material.color = hackedMeshColor;
            }
        }
    }

    private void EmitHackParticles(bool enable)
    {
        if (enable)
        {
            particlesObject.SetActive(true);
            if (!hackParticles.isPlaying)
            {
                hackParticles.Play();
            }
        }
        else
        {
            particlesObject.SetActive(false);
            if (hackParticles.isPlaying)
            {
                hackParticles.Stop();
            }
        }
    }

    float distance;
    private void DetectSubmarine()
    {
        distance = Mathf.Abs(Vector2.Distance(Coordinates.ConvertWorldToVector2(transform.position), submarine.coords.position));

        if (distance < detectionAlertRange)
        {
            if (currentTimeInRange <= timeInRangeBeforeAlert)
            {
                currentTimeInRange += Time.deltaTime;
            }
            else
            {
                alertIcon.transform.forward = GameManager.Instance.cameraController.cam.transform.forward;

                if (!alertFlag)
                {
                    alertFlag = true;
                    rangeDisplay.SetActive(true);
                    alertIcon.SetActive(true);
                }
            }  
        }
        else
        {
            currentTimeInRange = 0;
            if (alertFlag)
            {
                alertFlag = false;
                rangeDisplay.SetActive(false);
                alertIcon.SetActive(false);
            } 
        }
    }
}
