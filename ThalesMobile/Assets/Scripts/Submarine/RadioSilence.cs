using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSilence : CounterMeasure
{
    public override void LauchCounterMeasure(Submarine submarine)
    {
        base.LauchCounterMeasure(submarine);

        if (readyToUse)
        {
            if (!actionReached)
            {
                actionReached = true;
                submarine.currentDetectableState = DetectableState.cantBeDetected;
                submarine.PickRandomInterrestPoint();
            }
        }
        else
        {
            if (actionReached)
            {
                actionReached = false;
                submarine.currentDetectableState = DetectableState.undetected;
            }
        }
    }
}
