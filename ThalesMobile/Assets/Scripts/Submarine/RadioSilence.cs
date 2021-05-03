using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CounterMeasure/RadioSilence")]
public class RadioSilence : CounterMeasure
{
    public override IEnumerator CounterMeasureEffect(Submarine submarine)
    {
        submarine.currentDetectableState = DetectableState.cantBeDetected;
        submarine.PickRandomInterrestPoint();

        yield return new WaitForSeconds(duration);

        submarine.currentDetectableState = DetectableState.undetected;

        yield return base.CounterMeasureEffect(submarine);
    }
}
