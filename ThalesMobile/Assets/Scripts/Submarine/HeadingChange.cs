using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadingChange : CounterMeasure
{
    public override void LauchCounterMeasure(Submarine submarine)
    {
        base.LauchCounterMeasure(submarine);

        if (readyToUse)
        {
            if (!actionReached)
            {
                actionReached = true;
                submarine.PickRandomInterrestPoint();
            }
        }
        else
        {
            if (actionReached)
            {
                actionReached = false;
            }
        }
    }
}
