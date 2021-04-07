using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CounterMeasure : ScriptableObject
{
    [HideInInspector] public bool usingCounterMeasure = false;
    [HideInInspector] public bool cantUseAnotherCounterMeasure = false;
    [HideInInspector] public bool readyToUse = false;
    [HideInInspector] public bool inCooldown = false;
    [HideInInspector] public bool actionReached = false;

    private Submarine submarineRef;

    [SerializeField]
    float loadingTime;

    [SerializeField]
    float duration;

    [SerializeField]
    float cooldownTime;

    [SerializeField]
    float vigilanceCostValue;

    private List<Coroutine> coroutines;

    public virtual void Awake()
    {

    }

    IEnumerator CounterMeasureActivate()
    {
        yield return new WaitForSeconds(loadingTime);

        readyToUse = true;

        yield return new WaitForSeconds(duration);

        readyToUse = false;
        inCooldown = true;
        cantUseAnotherCounterMeasure = false;
        DecreaseViglance();

        yield return new WaitForSeconds(cooldownTime);

        inCooldown = false;
        usingCounterMeasure = false;
    }

    protected void UseCounterMeasure()
    {
        if (!usingCounterMeasure)
        {
            usingCounterMeasure = true;
            cantUseAnotherCounterMeasure = true;
            GameManager.Instance.ExternalStartCoroutine(CounterMeasureActivate());
            //coroutines.Add(GameManager.Instance.ExternalStartCoroutine(CounterMeasureActivate());
        }
    }

    public virtual void LauchCounterMeasure(Submarine submarine)
    {
        submarineRef = submarine;
        UseCounterMeasure();
        // Place here the behavior of the current Counter Measure. 
    }

    public virtual void DecreaseViglance()
    {
        submarineRef.currentVigilance -= vigilanceCostValue;
    }

    protected virtual void OnDestroy()
    {
        GameManager.Instance.ExternalStopCoroutine(CounterMeasureActivate());
    }
}
