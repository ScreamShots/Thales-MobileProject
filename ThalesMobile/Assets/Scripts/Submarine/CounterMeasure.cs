using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antoine Leroux - 08/04/2021 - Base for counter measure creation. 
/// </summary>
public abstract class CounterMeasure : ScriptableObject
{
    [HideInInspector] public bool readyToUse = true;

    [Header ("Counter Measure Parameters")]
    [SerializeField]
    protected float loadingTime;

    [SerializeField]
    protected float duration;

    [SerializeField]
    protected float cooldownTime;

    [SerializeField]
    protected float vigilanceCostValue;

    private Submarine submarineRef;

    //private List<Coroutine> coroutines;

    IEnumerator Buffer()
    {
        yield return new WaitForSeconds(loadingTime);
        GameManager.Instance.ExternalStartCoroutine(CounterMeasureEffect(submarineRef));
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        readyToUse = true;
        DecreaseViglance();
    }

    public virtual IEnumerator CounterMeasureEffect(Submarine submarine)
    {
        // Place counter measure effect here. 
        // Can place here a duration in the child class. 
        yield return null;
        GameManager.Instance.ExternalStartCoroutine(Cooldown());
    }

    public virtual void UseCounterMeasure(Submarine submarine)
    {
        submarineRef = submarine;

        readyToUse = false;
        GameManager.Instance.ExternalStartCoroutine(Buffer());       
    }

    protected void DecreaseViglance()
    {
        submarineRef.currentVigilance -= vigilanceCostValue;
    }

    protected virtual void OnDestroy()
    {
        GameManager.Instance.ExternalStopCoroutine(Buffer());
        GameManager.Instance.ExternalStopCoroutine(Cooldown());
        GameManager.Instance.ExternalStopCoroutine(CounterMeasureEffect(submarineRef));
    }
}
