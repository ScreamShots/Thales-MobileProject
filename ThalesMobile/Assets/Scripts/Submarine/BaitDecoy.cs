using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CounterMeasure/BaitDecoy")]
public class BaitDecoy : CounterMeasure
{
    private DecoyInstance decoyRef;

    [Header ("Decoy")]
    public float decoyAngle;
    public LayerMask layerMaskTarget;

    private int leftOrRight;
    private int randomDirection;

    public override IEnumerator CounterMeasureEffect(Submarine submarine)
    {
        decoyRef = submarine.decoy;

        decoyRef.layerMaskTarget = layerMaskTarget;
        ChooseRandomSide();
        ChooseObjectDirection();       

        submarine.decoy.decoyIsActive = true;
        if (!decoyRef.levelManager.submarineEntitiesInScene.Contains(decoyRef))
        {
            decoyRef.levelManager.submarineEntitiesInScene.Add(decoyRef);
        }

        yield return new WaitForSeconds(duration);

        submarine.decoy.decoyIsActive = false;
        if (decoyRef.levelManager.submarineEntitiesInScene.Contains(decoyRef))
        {
            decoyRef.levelManager.submarineEntitiesInScene.Remove(decoyRef);
        }

        yield return base.CounterMeasureEffect(submarine);
    }

    private void ChooseRandomSide()
    {
        // Choose if the decoy direction will be left of right.
        leftOrRight = Random.Range(0, 2);

        if (leftOrRight == 1)
        {
            decoyAngle = -decoyAngle;
        }
        decoyRef.decoyAngle = decoyAngle;
    }

    private void ChooseObjectDirection()
    {
        // Choose a random direction between submarine direction and decoy direction. 
        randomDirection = Random.Range(0, 2);
        decoyRef.randomDirection = randomDirection;
    }
}
