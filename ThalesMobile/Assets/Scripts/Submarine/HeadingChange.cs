using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CounterMeasure/HeadingChange")]
public class HeadingChange : CounterMeasure
{
    public override IEnumerator CounterMeasureEffect(Submarine submarine)
    {
        submarine.PickRandomInterrestPoint();

        yield return null;

        base.CounterMeasureEffect(submarine);
    }
}
