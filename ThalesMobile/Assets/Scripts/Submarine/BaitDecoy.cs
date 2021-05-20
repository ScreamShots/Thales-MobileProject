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
    public List<int> randomAngle;
    public LayerMask layerMaskTarget;

    private int leftOrRight;
    private int randomDirection;

    public override IEnumerator CounterMeasureEffect(Submarine submarine)
    {
        randomAngle.Clear();
        randomAngle.Add(0);
        randomAngle.Add(1);
        randomAngle.Add(2);
        randomAngle.Add(3);

        decoyRef = submarine.decoy;
        decoyRef2 = submarine.decoy2;
        decoyRef3 = submarine.decoy3;

        decoyRef.layerMaskTarget = layerMaskTarget;
        decoyRef2.layerMaskTarget = layerMaskTarget;
        decoyRef3.layerMaskTarget = layerMaskTarget;

        ChooseRandomSide();
        ChooseObjectDirection();       

        submarine.decoy.decoyIsActive = true;
        submarine.decoy2.decoyIsActive = true;
        submarine.decoy3.decoyIsActive = true;
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

        leftOrRight = Random.Range(0, randomAngle.Count);
        randomAngle.Remove(leftOrRight);
        decoyRef.decoyAngle = decoysAngle[leftOrRight];
        decoyRef2.decoyAngle = decoysAngle[0];
        randomAngle.Remove(0);
        decoyRef3.decoyAngle = decoysAngle[0];
        randomAngle.Remove(0);
    }

    private void ChooseObjectDirection()
    {
        // Choose a random direction between submarine direction and decoy direction. 
        //randomDirection = Random.Range(0, 2);
        decoyRef.randomDirection = randomAngle[0];
    }
}
