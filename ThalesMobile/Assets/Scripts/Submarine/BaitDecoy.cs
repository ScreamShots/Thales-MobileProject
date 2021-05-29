using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CounterMeasure/BaitDecoy")]
public class BaitDecoy : CounterMeasure
{
    private DecoyInstance decoyRef;
    private DecoyInstance decoyRef2;
    private DecoyInstance decoyRef3;

    [Header ("Decoy")]
    public float decoyAngle;
    public List<float> decoysAngle;
    public List<int> randomAnglelistIndex;
    public LayerMask layerMaskTarget;

    private int randomAngleIndex;
    private int randomDirection;

    public override IEnumerator CounterMeasureEffect(Submarine submarine)
    {
        randomAnglelistIndex.Clear();
        randomAnglelistIndex.Add(0);
        randomAnglelistIndex.Add(1);
        randomAnglelistIndex.Add(2);
        randomAnglelistIndex.Add(3);

        decoyRef = submarine.decoy;
        decoyRef2 = submarine.decoy2;
        decoyRef3 = submarine.decoy3;

        decoyRef.layerMaskTarget = layerMaskTarget;
        decoyRef2.layerMaskTarget = layerMaskTarget;
        decoyRef3.layerMaskTarget = layerMaskTarget;

        ChooseRandomSide();
        

        submarine.decoy.decoyIsActive = true;       
        submarine.decoy2.decoyIsActive = true;
        submarine.decoy3.decoyIsActive = true;

        if (submarine.linkedGlobalDetectionPoint.activated)
        {
            if (submarine.decoy.linkedGlobalDetectionPoint.activated) submarine.decoy.linkedGlobalDetectionPoint.UpdatePoint();
            else submarine.decoy.linkedGlobalDetectionPoint.InitPoint();

            if (submarine.decoy2.linkedGlobalDetectionPoint.activated) submarine.decoy2.linkedGlobalDetectionPoint.UpdatePoint();
            else submarine.decoy2.linkedGlobalDetectionPoint.InitPoint();

            if (submarine.decoy3.linkedGlobalDetectionPoint.activated) submarine.decoy3.linkedGlobalDetectionPoint.UpdatePoint();
            else submarine.decoy3.linkedGlobalDetectionPoint.InitPoint();
        }

        submarine.decoy.linkedGlobalDetectionPoint.detectionState = submarine.linkedGlobalDetectionPoint.detectionState;
        submarine.decoy2.linkedGlobalDetectionPoint.detectionState = submarine.linkedGlobalDetectionPoint.detectionState;
        submarine.decoy3.linkedGlobalDetectionPoint.detectionState = submarine.linkedGlobalDetectionPoint.detectionState;

        submarine.isDecoyMoving = true;
        if (!decoyRef.levelManager.submarineEntitiesInScene.Contains(decoyRef))
        {
            decoyRef.levelManager.submarineEntitiesInScene.Add(decoyRef);
            decoyRef.levelManager.submarineEntitiesInScene.Add(decoyRef2);
            decoyRef.levelManager.submarineEntitiesInScene.Add(decoyRef3);
        }

        yield return new WaitForSeconds(duration);

        submarine.decoy.decoyIsActive = false;
        submarine.decoy2.decoyIsActive = false;
        submarine.decoy3.decoyIsActive = false;
        submarine.isDecoyMoving = false;
        if (decoyRef.levelManager.submarineEntitiesInScene.Contains(decoyRef))
        {
            decoyRef.levelManager.submarineEntitiesInScene.Remove(decoyRef);
            decoyRef.levelManager.submarineEntitiesInScene.Remove(decoyRef2);
            decoyRef.levelManager.submarineEntitiesInScene.Remove(decoyRef3);
        }

        yield return base.CounterMeasureEffect(submarine);
    }

    private void ChooseRandomSide()
    {
        // Choose if the decoy direction will be left of right.

        Debug.Log(randomAnglelistIndex.Count);

        randomAngleIndex = randomAnglelistIndex[Random.Range(0, randomAnglelistIndex.Count)];
        decoyRef.decoyAngle = decoysAngle[randomAngleIndex];
        randomAnglelistIndex.Remove(randomAngleIndex);
        Debug.Log(decoysAngle[randomAngleIndex]);

        randomAngleIndex = randomAnglelistIndex[Random.Range(0, randomAnglelistIndex.Count)];
        decoyRef2.decoyAngle = decoysAngle[randomAngleIndex];
        randomAnglelistIndex.Remove(randomAngleIndex);
        Debug.Log(decoysAngle[randomAngleIndex]);

        randomAngleIndex = randomAnglelistIndex[Random.Range(0, randomAnglelistIndex.Count)];
        decoyRef3.decoyAngle = decoysAngle[randomAngleIndex];
        randomAnglelistIndex.Remove(randomAngleIndex);
        Debug.Log(decoysAngle[randomAngleIndex]);
    }
}
