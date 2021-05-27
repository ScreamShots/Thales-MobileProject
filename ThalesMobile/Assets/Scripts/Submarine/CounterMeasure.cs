using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antoine Leroux - 08/04/2021 - Base for counter measure creation. 
/// </summary>
public abstract class CounterMeasure : ScriptableObject
{
    public bool readyToUse = true;

    [Header ("Counter Measure Parameters")]
    [SerializeField]
    public float loadingTime;

    [SerializeField]
    public float duration;

    [SerializeField]
    protected float cooldownTime;

    [SerializeField]
    protected float vigilanceCostValue;

    private Submarine submarineRef;

    protected List<Coroutine> allCoroutines = new List<Coroutine>();
    Coroutine bufferCoroutine = null;
    Coroutine counterMeasureEffectCoroutine = null;
    Coroutine cooldownCoroutine = null;

    IEnumerator Buffer()
    {
        yield return new WaitForSeconds(loadingTime);
        counterMeasureEffectCoroutine = GameManager.Instance.ExternalStartCoroutine(CounterMeasureEffect(submarineRef));
        allCoroutines.Add(counterMeasureEffectCoroutine);
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
        cooldownCoroutine = GameManager.Instance.ExternalStartCoroutine(Cooldown());
        allCoroutines.Add(cooldownCoroutine);
    }

    public virtual void UseCounterMeasure(Submarine submarine)
    {
        submarineRef = submarine;

        readyToUse = false;
        bufferCoroutine = GameManager.Instance.ExternalStartCoroutine(Buffer());
        allCoroutines.Add(bufferCoroutine);
    }

    protected void DecreaseViglance()
    {
        submarineRef.currentVigilance -= vigilanceCostValue;
    }

    protected virtual void OnDestroy()
    {
        foreach(Coroutine coroutine in allCoroutines)
        {
            if (coroutine != null) GameManager.Instance.ExternalStopCoroutine(coroutine);
        }
    }
}
