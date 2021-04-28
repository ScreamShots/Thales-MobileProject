using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;

/// <summary>
/// Antoine Leroux - 07/04/2021 - Vigilance State is an enum describing the current submarine state.
/// </summary>
public enum VigilanceState
{
    calm,
    worried,
    panicked
}

/// <summary>
/// Antoine Leroux - 28/03/2021 - Script relative to the movement and behavior of SubMarine entity. 
/// </summary>
public class Submarine : DetectableOceanEntity
{
    private Transform _transform;

    [Header("References")]
    private Ship ship;
    private LevelManager levelManager;
    private Environnement environnement;

    [Header("Movement")]
    public float maxSpeed;
    public float acceleration;
    [HideInInspector] public float currentSpeed;
    private bool movingToNextPoint;

    [Header("Vigilance")]
    public float currentVigilance = 0f;
    public VigilanceState currentState;

    [Header("Vigilance State")]
    public Vector2 calmStateValues;
    public Vector2 worriedStateValues;
    public Vector2 panickedStateValues;
    private bool reachWorriedState;

    [Header("Submarine Range")]
    public float detectionRangeCalm;
    public float detectionRangeWorried;
    public float detectionRangePanicked;
    private float currentRange;
    public GameObject rangeVisual;

    [Header("Vigilance Incrase Values")]
    public float sonobuoyVigiIncr;
    public float fregateStationaryVigiIncr;
    public float fregateMoveVigiIncr;
    private bool submarineDetectFregate;
    [HideInInspector] public List<Transform> sonobuoys;
    [HideInInspector] public List<float> sonobuoysDistance;

    [Header("Materials Detection")]
    public MeshRenderer submarineRenderer;
    public Material baseMaterial;
    public Material detectedByFlashMaterial;

    [Header("Counter Measures")]
    public DecoyInstance decoy;
    private bool decoyIsCreateFlag;
    public List<CounterMeasure> counterMeasures;

    [Header("Objectif")]
    public int pointsToHack;
    private int pointsHacked = 0;
    private float hackingTimer = 0f;

    [Space]
    public List<InterestPoint> interestPoints;
    private InterestPoint nextInterestPoint;
    private Vector2 nextInterestPointPosition;
    private int randomNumber;

    [Header("Smart Move")]
    public float minRange;
    public int subZone12Subdivision;
    public int subZone3SubSubdivision;
    public List<Transform> bioElements;

    public int avoidEffectSliceReach;
    public float intermediatePosRefreshRate;
    public float distanceToRefrehIntemediatePos;
    public float benefPointFactorBioCalm, benefPointFactorBioWorried, benefPointFactorBioPanicked;
    public float beneftPointFactorSonobuoyCalm, beneftPointFactorSonobuoyWorried, beneftPointFactorSonobuoyPanicked;
    public float beneftPointFactorSeaWayCalm, beneftPointFactorSeaWayWorried, beneftPointFactorSeaWayPanicked;
    public float beneftPointFactorSeaTurbulentCalm, beneftPointFactorSeaTurbulentWorried, beneftPointFactorSeaTurbulentPanicked;
    public float beneftPointFactorWindyZoneCalm, beneftPointFactorWindyZoneWorried, beneftPointFactorWindyZonePanicked;
    public float distanceFactorWeightWhileCalm, distanceFactorWeightWhileWorried, distanceFactorWeightWhilePanicked;

    private Vector2 targetDirection;
    private Vector2 nextIntermediatePosition;
    private Vector2 intermediateDirection;
    private float subZoneAngleWidth12;
    private float subZoneAngleWidth3;
    private float timeBeforeNextRefresh;
    public bool isSubmarineDisplayed;

    private void Start()
    {
        levelManager = GameManager.Instance.levelManager;
        environnement = levelManager.environnement;

        levelManager.submarineEntitiesInScene.Add(this);
        levelManager.enemyEntitiesInScene.Add(this);

        _transform = transform;
        coords.position = Coordinates.ConvertWorldToVector2(_transform.position);
        currentSeaLevel = SeaLevel.submarine;
        PickRandomInterrestPoint();      
        ship = Object.FindObjectOfType<Ship>();

        subZoneAngleWidth12 = 360 / subZone12Subdivision;
        subZoneAngleWidth3 = 360 / (subZone12Subdivision * subZone3SubSubdivision);

        for (int i = 0; i < levelManager.submarineEntitiesInScene.Count; i++)
        {
            if(levelManager.submarineEntitiesInScene[i].GetType() != typeof(Submarine))
            {
                bioElements.Add(levelManager.submarineEntitiesInScene[i].transform);
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        // Movement.
        if (!decoy.decoyIsActive)
        {
            UpdateInterestPoint();
            decoyIsCreateFlag = false;
        }
        else
        {
            SubmarineDecoyMovement();

            if (!decoyIsCreateFlag)
            {
                decoyIsCreateFlag = true;
                PickRandomInterrestPoint();
            }
        }

        // Vigilance.
        UpdateState();
        UpdateSubmarineRange();
        DetectFregate();
        DetectSonobuoy();

        // Counter Measures.
        if (currentVigilance >= 0)
        {
            UpdateCounterMeasures();
        }
    }

    #region Movement
    public void PickRandomInterrestPoint()
    {
        // If the submarine currently hacking a point.
        if (hackingTimer > 0)
        {
            nextInterestPoint.currentHackState = HackState.unhacked;
            nextInterestPoint.hackProgression = 0f;
            hackingTimer = 0;
            randomNumber = Random.Range(0, interestPoints.Count);
            nextInterestPoint = interestPoints[randomNumber];
            movingToNextPoint = true;
        }
        else
        {
            randomNumber = Random.Range(0, interestPoints.Count);
            nextInterestPoint = interestPoints[randomNumber];
            movingToNextPoint = true;
        }
    }

    private void UpdateInterestPoint()
    {
        if (pointsHacked < pointsToHack)
        {
            nextInterestPointPosition = Coordinates.ConvertWorldToVector2(nextInterestPoint.transform.position);
            // Move Submarine to the target point.
            if (nextInterestPoint != null && movingToNextPoint)
            {
                targetDirection = pathDirection;
                targetDirection.Normalize();

                if (Vector2.Distance(coords.position, nextIntermediatePosition) < distanceToRefrehIntemediatePos)
                {
                    RefreshIntermediatePosition();
                }

                if (timeBeforeNextRefresh > 0)
                {
                    timeBeforeNextRefresh -= Time.deltaTime;
                }
                else
                {
                    RefreshIntermediatePosition();
                    timeBeforeNextRefresh = intermediatePosRefreshRate;
                }

                Move(nextInterestPointPosition);
            }

            // Hacking the interrest point with hacking time specific to the interrest point.
            if ((nextInterestPointPosition - coords.position).magnitude < nextInterestPoint.hackingRange)
            {
                Capture();
            }
            else
            {
                pathDestination = nextInterestPointPosition;
                UpdatePath();
            }
        }
        else
        {
            // Player lose.
        }
    }

    private void Capture()
    {
        hackingTimer += Time.deltaTime;
        nextInterestPoint.currentHackState = HackState.inHack;
        nextInterestPoint.hackProgression = ((hackingTimer - 0) / nextInterestPoint.hackTime) * 100f;

        if (hackingTimer >= nextInterestPoint.hackTime)
        {
            hackingTimer = 0;
            nextInterestPoint.currentHackState = HackState.doneHack;
            interestPoints.RemoveAt(randomNumber);
            pointsHacked++;
            PickRandomInterrestPoint();
        }
    }

    private void SubmarineDecoyMovement()
    {
        // The submarine still going in his direction
        if (decoy.randomDirection == 0)
        {
            coords.direction = coords.direction;
            coords.position += coords.direction.normalized * Time.deltaTime * currentSpeed;
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
        }
        // The submarine go in decoy angle direction 
        else
        {
            coords.direction = Coordinates.ConvertWorldToVector2(Quaternion.Euler(0, decoy.decoyAngle, 0) * Coordinates.ConvertVector2ToWorld(coords.direction.normalized));
            coords.position += coords.direction * Time.deltaTime * currentSpeed;
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
        }
    }

    public override void Move(Vector2 targetPosition)
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        //Calculate direction to target and store it in coords.
        //coords.direction = targetPosition - coords.position;
        coords.direction = intermediateDirection;

        //Update the plane's position.
        coords.position += coords.direction.normalized * currentSpeed * Time.deltaTime;

        //Store the new position in the coords.
        _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

        //Make the submarine face his current direction.
        _transform.rotation = Quaternion.LookRotation(Coordinates.ConvertVector2ToWorld(coords.direction));   

        if ((targetPosition - coords.position).magnitude < 0.1f)
        {
            movingToNextPoint = false;
            currentSpeed = 0;
        }
    }

    public override void PathFinding()
    {

    }
    #endregion

    #region SmartMove
    private void RefreshIntermediatePosition()
    {
        nextIntermediatePosition = FindNextIntermediatePosition();
        intermediateDirection = nextIntermediatePosition - coords.position;
        //intermediateDirection.Normalize();
    }

    private List<SubZone> allSubZones = new List<SubZone>();

    private Vector2 FindNextIntermediatePosition()
    {
        allSubZones.Clear();
        float startAngle = Vector2.SignedAngle(Vector2.right, targetDirection) - (360 / (subZone12Subdivision * subZone3SubSubdivision));
        for (int i = 0; i < subZone12Subdivision; i++)
        {
            allSubZones.Add(new SubZone(GetNormAngle(startAngle + i * subZoneAngleWidth12), GetNormAngle(startAngle + (i + 1) * subZoneAngleWidth12), minRange, detectionRangeCalm, i, "SZ_" + i + "_0", this));
            if (isSubmarineDisplayed)
                Debug.DrawRay(Coordinates.ConvertVector2ToWorld(coords.position),
                    Coordinates.ConvertVector2ToWorld(GetDirectionFromAngle(allSubZones[allSubZones.Count - 1].minAngle) * detectionRangeCalm),
                    Color.green);

            if (currentState == VigilanceState.worried
        || currentState == VigilanceState.panicked)
            {
                allSubZones.Add(new SubZone(GetNormAngle(startAngle + i * subZoneAngleWidth12), GetNormAngle(startAngle + (i + 1) * subZoneAngleWidth12), detectionRangeCalm, detectionRangeWorried, i, "SZ_" + i + "_1", this));

                if (isSubmarineDisplayed)
                    Debug.DrawRay(Coordinates.ConvertVector2ToWorld(coords.position + GetDirectionFromAngle(allSubZones[allSubZones.Count - 1].minAngle) * detectionRangeCalm),
                        Coordinates.ConvertVector2ToWorld(GetDirectionFromAngle(allSubZones[allSubZones.Count - 1].minAngle) * (detectionRangeWorried - detectionRangeCalm)),
                        Color.cyan);


                if (currentState == VigilanceState.panicked)
                {
                    for (int y = 0; y < subZone3SubSubdivision; y++)
                    {
                        allSubZones.Add(new SubZone(GetNormAngle(startAngle + i * subZoneAngleWidth12 + y * subZoneAngleWidth3), GetNormAngle(startAngle + i * subZoneAngleWidth12 + (y + 1) * subZoneAngleWidth3), detectionRangeWorried, detectionRangePanicked, i, "SZ_" + i + "_2." + y, this));
                        if (isSubmarineDisplayed)
                            Debug.DrawRay(Coordinates.ConvertVector2ToWorld(coords.position + GetDirectionFromAngle(allSubZones[allSubZones.Count - 1].minAngle) * detectionRangeWorried), Coordinates.ConvertVector2ToWorld(GetDirectionFromAngle(allSubZones[allSubZones.Count - 1].minAngle) * (detectionRangePanicked - detectionRangeWorried)), Color.red);
                    }
                }
            }
            /*if (isSubmarineDisplayed)
                circleGismos.Add(new CircleGizmo(coords.position, detectionRangeCalm, Color.green));*/

            if (currentState == VigilanceState.worried
            || currentState == VigilanceState.panicked)
            {
                /*if (isSubmarineDisplayed)
                    circleGismos.Add(new CircleGizmo(coords.position, detectionRangeWorried, Color.cyan));*/


                if (currentState == VigilanceState.panicked)
                {
                    /*if (isSubmarineDisplayed)
                        circleGismos.Add(new CircleGizmo(coords.position, detectionRangePanicked, Color.red));*/
                }
            }
        }
        SubZone bestSubZone = allSubZones[0];

        for (int i = 0; i < allSubZones.Count; i++)
        {
            allSubZones[i].weight = GetSubZoneWeight(allSubZones[i]);
        }

        for (int i = 0; i < allSubZones.Count; i++)
        {
            if (allSubZones[i].needToBeAvoided && avoidEffectSliceReach > 0)
            {
                for (int y = 0; y < allSubZones.Count; y++)
                {
                    if (allSubZones[y].sliceIndex == allSubZones[i].sliceIndex)
                    {
                        allSubZones[y].weight = -1000;
                        //sphereGizmos.Add(new SphereGizmo(allSubZones[y].zoneCenterPos, 0.4f, Color.black));
                    }

                    for (int s = 1; s < avoidEffectSliceReach; s++)
                    {
                        if ((allSubZones[i].sliceIndex + s) < subZone12Subdivision)
                        {
                            if (allSubZones[y].sliceIndex == (allSubZones[i].sliceIndex + s))
                            {
                                allSubZones[y].weight = -1000;
                                //sphereGizmos.Add(new SphereGizmo(allSubZones[y].zoneCenterPos, 0.4f, Color.black));
                            }
                        }
                        else
                        {
                            if (allSubZones[y].sliceIndex == s - (subZone12Subdivision - allSubZones[i].sliceIndex))
                            {
                                allSubZones[y].weight = -1000;
                                //sphereGizmos.Add(new SphereGizmo(allSubZones[y].zoneCenterPos, 0.4f, Color.black));
                            }
                        }


                        if ((allSubZones[i].sliceIndex - s) >= 0)
                        {
                            if (allSubZones[y].sliceIndex == (allSubZones[i].sliceIndex - s))
                            {
                                allSubZones[y].weight = -1000;
                                //sphereGizmos.Add(new SphereGizmo(allSubZones[y].zoneCenterPos, 0.4f, Color.black));
                            }
                        }
                        else
                        {
                            if (allSubZones[y].sliceIndex == (subZone12Subdivision - (s - allSubZones[i].sliceIndex)))
                            {
                                allSubZones[y].weight = -1000;
                                //sphereGizmos.Add(new SphereGizmo(allSubZones[y].zoneCenterPos, 0.4f, Color.black));
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < allSubZones.Count; i++)
        {
            //sphereGizmos.Add(new SphereGizmo(allSubZones[i].zoneCenterPos, 0.2f, Color.Lerp(Color.red, Color.yellow, (allSubZones[i].weight + 15) / 20)));

            if (bestSubZone == null || allSubZones[i].weight > bestSubZone.weight)
            {
                bestSubZone = allSubZones[i];
            }
        }

        return bestSubZone.zoneCenterPos;
    }

    float pointDistance;
    float pointRelativeAngle;
    Vector2 subZoneDirection;
    private float GetSubZoneWeight(SubZone subZone)
    {
        float weight = 0;

        subZoneDirection = GetDirectionFromAngle(GetNormAngle(subZone.minAngle + Mathf.DeltaAngle(subZone.minAngle, subZone.maxAngle) * 0.5f));

        for (int o = 0; o < bioElements.Count; o++)
        {
            pointDistance = Vector2.Distance(Coordinates.ConvertWorldToVector2(bioElements[o].position), coords.position);
            pointRelativeAngle = Vector2.SignedAngle(Vector2.right, Coordinates.ConvertWorldToVector2(bioElements[o].position) - coords.position);
            if (pointDistance < subZone.maxRange && pointDistance >= subZone.minRange && IsBetweenAngle(pointRelativeAngle, subZone.minAngle, subZone.maxAngle))
            {
                weight += currentState == VigilanceState.calm ? benefPointFactorBioCalm : 0;
                weight += currentState == VigilanceState.worried ? benefPointFactorBioWorried : 0;
                weight += currentState == VigilanceState.panicked ? benefPointFactorBioPanicked : 0;
            }
        }

        switch (currentState)
        {
            case VigilanceState.calm:
                weight += Mathf.Cos(Mathf.Deg2Rad * Vector2.Angle(targetDirection, subZoneDirection)) * distanceFactorWeightWhileCalm;
                break;

            case VigilanceState.worried:
                weight += Mathf.Cos(Mathf.Deg2Rad * Vector2.Angle(targetDirection, subZoneDirection)) * distanceFactorWeightWhileWorried;
                break;

            case VigilanceState.panicked:
                weight += Mathf.Cos(Mathf.Deg2Rad * Vector2.Angle(targetDirection, subZoneDirection)) * distanceFactorWeightWhilePanicked;
                break;
        }

        if (environnement.zones[environnement.ZoneIn(coords.position)].state == ZoneState.SeaWay)
        {
            switch (currentState)
            {
                case VigilanceState.calm:
                    weight += beneftPointFactorSeaWayCalm;
                    break;

                case VigilanceState.worried:
                    weight += beneftPointFactorSeaWayWorried;
                    break;

                case VigilanceState.panicked:
                    weight += beneftPointFactorSeaWayPanicked;
                    break;
            }
        }
        else if (environnement.zones[environnement.ZoneIn(coords.position)].state == ZoneState.WindyZone)
        {
            switch (currentState)
            {
                case VigilanceState.calm:
                    weight += beneftPointFactorWindyZoneCalm;
                    break;

                case VigilanceState.worried:
                    weight += beneftPointFactorWindyZoneWorried;
                    break;

                case VigilanceState.panicked:
                    weight += beneftPointFactorWindyZonePanicked;
                    break;
            }
        }
        else if (environnement.zones[environnement.ZoneIn(coords.position)].state == ZoneState.SeaTurbulent)
        {
            switch (currentState)
            {
                case VigilanceState.calm:
                    weight += beneftPointFactorSeaTurbulentCalm;
                    break;

                case VigilanceState.worried:
                    weight += beneftPointFactorSeaTurbulentWorried;
                    break;

                case VigilanceState.panicked:
                    weight += beneftPointFactorSeaTurbulentPanicked;
                    break;
            }
        }

        pointDistance = Vector2.Distance(ship.coords.position, coords.position);
        pointRelativeAngle = Vector2.SignedAngle(Vector2.right, ship.coords.position - coords.position);
        if (pointDistance < subZone.maxRange && pointDistance >= subZone.minRange && IsBetweenAngle(pointRelativeAngle, subZone.minAngle, subZone.maxAngle))
        {
            weight = -1000;
            subZone.needToBeAvoided = true;
        }

        

        for (int b = 0; b < sonobuoys.Count; b++)
        {
            pointDistance = Vector2.Distance(Coordinates.ConvertWorldToVector2(sonobuoys[b].position), coords.position);
            pointRelativeAngle = Vector2.SignedAngle(Vector2.right, Coordinates.ConvertWorldToVector2(sonobuoys[b].position) - coords.position);
            if (pointDistance < subZone.maxRange && pointDistance >= subZone.minRange && IsBetweenAngle(pointRelativeAngle, subZone.minAngle, subZone.maxAngle))
            {
                switch (currentState)
                {
                    case VigilanceState.calm:
                        weight += beneftPointFactorSonobuoyCalm;
                        break;

                    case VigilanceState.worried:
                        weight += beneftPointFactorSonobuoyWorried;
                        break;

                    case VigilanceState.panicked:
                        weight += beneftPointFactorSonobuoyPanicked;
                        break;
                }
                
                subZone.needToBeAvoided = true;
            }
        }

        return weight;
    }

    private float GetNormAngle(float angle)
    {
        float newAngle = angle;

        if (angle > 180)
        {
            newAngle = angle - 360;
        }

        if (angle <= -180)
        {
            newAngle = angle + 360;
        }
        return newAngle;
    }

    private bool IsBetweenAngle(float angle, float mininmumAngle, float maximumAngle)
    {
        bool isBetween = false;
        if (mininmumAngle > maximumAngle)
        {
            if (angle >= mininmumAngle || angle < maximumAngle)
            {
                isBetween = true;
            }
        }
        else
        {
            if (angle >= mininmumAngle && angle < maximumAngle)
            {
                isBetween = true;
            }
        }

        //Debug.Log("min : " + mininmumAngle + ", max : " + maximumAngle + ", Angle : " + angle + ". Between : " + isBetween);
        return isBetween;
    }

    private Vector2 GetDirectionFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public class SubZone
    {
        public Vector2 zoneCenterPos;
        public float minAngle, maxAngle;
        public float minRange, maxRange;
        public float weight;
        public int sliceIndex;
        public string identity;
        public bool needToBeAvoided;

        public SubZone(float _minAngle, float _maxAngle, float _minRange, float _maxRange, int _sliceIndex, string _identity, Submarine submarine)
        {
            minAngle = _minAngle;
            maxAngle = _maxAngle;
            minRange = _minRange;
            maxRange = _maxRange;
            sliceIndex = _sliceIndex;
            identity = _identity;
            needToBeAvoided = false;
            weight = -666;

            zoneCenterPos = submarine.coords.position + submarine.GetDirectionFromAngle(submarine.GetNormAngle(minAngle + Mathf.DeltaAngle(minAngle, maxAngle) * 0.5f)) * (maxRange + minRange) * 0.5f;
        }
    }

    /*private List<CircleGizmo> circleGismos = new List<CircleGizmo>();
    private List<SphereGizmo> sphereGizmos = new List<SphereGizmo>();
    private void OnDrawGizmos()
    {
        if(isSubmarineDisplayed)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Coordinates.ConvertVector2ToWorld(nextIntermediatePosition), 0.2f);

            for (int i = 0; i < circleGismos.Count; i++)
            {
                Gizmos.color = circleGismos[i].color;
                Gizmos.DrawWireSphere(Coordinates.ConvertVector2ToWorld(circleGismos[i].seaPosition), circleGismos[i].range);
            }

            for (int i = 0; i < sphereGizmos.Count; i++)
            {
                Gizmos.color = sphereGizmos[i].color;
                Gizmos.DrawSphere(Coordinates.ConvertVector2ToWorld(sphereGizmos[i].seaPosition), sphereGizmos[i].radius);
            }

            circleGismos.Clear();
            sphereGizmos.Clear();
        }
    }

    public class CircleGizmo
    {
        public float range;
        public Vector2 seaPosition;
        public Color color;

        public CircleGizmo(Vector2 pos, float radius, Color _color)
        {
            seaPosition = pos;
            color = _color;
            range = radius;
        }
    }
    public class SphereGizmo
    {
        public float radius;
        public Vector2 seaPosition;
        public Color color;

        public SphereGizmo(Vector2 pos, float _radius, Color _color)
        {
            seaPosition = pos;
            color = _color;
            radius = _radius;
        }
    }*/
    #endregion

    #region Vigilance
    private void UpdateState()
    {
        if (currentVigilance >= calmStateValues.x && currentVigilance < calmStateValues.y && !reachWorriedState)
        {
            currentState = VigilanceState.calm;
        }
        else if ((currentVigilance >= worriedStateValues.x && currentVigilance < worriedStateValues.y) || (currentVigilance >= calmStateValues.x && currentVigilance < calmStateValues.y && reachWorriedState))
        {
            currentState = VigilanceState.worried;
            reachWorriedState = true;
        }
        else if (currentVigilance >= panickedStateValues.x && currentVigilance <= panickedStateValues.y)
        {
            currentState = VigilanceState.panicked;
        }

        if (currentVigilance >= 100)
        {
            currentVigilance = 100;
        }

        if (currentVigilance <= 0)
        {
            currentVigilance = 0;
        }
    }

    private void UpdateSubmarineRange()
    {
        if (currentState == VigilanceState.calm)
        {
            rangeVisual.transform.localScale = new Vector2(detectionRangeCalm * 2, detectionRangeCalm * 2);
            currentRange = detectionRangeCalm;
        }
        else if (currentState == VigilanceState.worried)
        {
            rangeVisual.transform.localScale = new Vector2(detectionRangeWorried * 2, detectionRangeWorried * 2);
            currentRange = detectionRangeWorried;
        }
        else if (currentState == VigilanceState.panicked)
        {
            rangeVisual.transform.localScale = new Vector2(detectionRangePanicked * 2, detectionRangePanicked * 2);
            currentRange = detectionRangePanicked;
        }
    }

    private void IncreaseVigilance(float valuePerSecond)
    {
        currentVigilance += valuePerSecond * Time.deltaTime;
    }

    private void DetectFregate()
    {
        float distanceFromFregate = Vector3.Distance(transform.position, ship.transform.position);

        if (distanceFromFregate < currentRange)
        {
            submarineDetectFregate = true;

            if (ship.currentTargetPoint != ship.nullVector)
            {
                IncreaseVigilance(fregateMoveVigiIncr);
            }
            else
            {
                IncreaseVigilance(fregateStationaryVigiIncr);
            }
        }
        else
        {
            submarineDetectFregate = false;
        }
    }

    private void DetectSonobuoy()
    {
        sonobuoys.Clear();

        for (int x = 0; x < GameManager.Instance.levelManager.sonobuoysInScene.Count; x++)
        {
            sonobuoys.Add(GameManager.Instance.levelManager.sonobuoysInScene[x].transform);
        }

        sonobuoysDistance = new List<float>(new float[sonobuoys.Count]);

        for (int x = 0; x < sonobuoys.Count; x++)
        {
            sonobuoysDistance[x] = Vector3.Distance(transform.position, sonobuoys[x].position);

            if (sonobuoysDistance[x] < currentRange)
            {
                IncreaseVigilance(sonobuoyVigiIncr);
            }
        }
    }

    // Use this function from flash script to change the submarine material during a time x;
    public void MaterialChangedByFlash(float timeMaterialChanged)
    {
        ChangeMaterialOverTime(timeMaterialChanged);
    }

    private IEnumerator ChangeMaterialOverTime(float time)
    {
        submarineRenderer.material = detectedByFlashMaterial;
        yield return new WaitForSeconds(time);
        submarineRenderer.material = baseMaterial;
    }
    #endregion

    #region CounterMeasures
    private void UpdateCounterMeasures()
    {
        // Lauch Heading Change counter measure.
        if ((currentState == VigilanceState.worried || currentState == VigilanceState.panicked) && submarineDetectFregate)
        {
            if (!UsingCounterMeasure())
            {
                counterMeasures[0].UseCounterMeasure(this);
            }
        }
        // Lauch Radio Silence counter measure.
        if (currentVigilance >= 100)
        {
            if (!UsingCounterMeasure())
            {
                counterMeasures[1].UseCounterMeasure(this);
            }
        }
        // Lauch Bait Decoy counter measure.
        if ((currentState == VigilanceState.worried || currentState == VigilanceState.panicked) && currentDetectableState == DetectableState.revealed)
        {
            if (!UsingCounterMeasure())
            {
                counterMeasures[2].UseCounterMeasure(this);
            }
        }
    }

    private bool UsingCounterMeasure()
    {
        for (int x = 0; x < counterMeasures.Count; x++)
        {
            if (!counterMeasures[x].readyToUse)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
