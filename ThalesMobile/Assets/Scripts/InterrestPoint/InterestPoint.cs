using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antoine Leroux - 28/03/2021 - Hack State is an enum describing the interrest point hacking state.
/// </summary>
public enum HackState
{
    unhacked,
    inHack,
    doneHack
}


/// <summary>
/// Antoine Leroux - 28/03/2021 - InterrestPoint is the class for every interrest point. 
/// </summary>
public class InterestPoint : MonoBehaviour
{
    public HackState currentHackState;
    public float hackProgression;
    public float hackTime;
    public float hackingRange;
}
