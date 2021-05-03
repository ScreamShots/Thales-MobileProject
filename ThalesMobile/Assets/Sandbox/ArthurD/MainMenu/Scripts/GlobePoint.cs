using UnityEngine;

[System.Serializable]
public class GlobePoint
{
    [Header("Globe Info")]
    public Vector2 pointCoord = Vector2.zero;
    [Space(10)]
    public GameObject button;
    public RectTransform buttonrectTrans;

    [Header("Mision Data")]
    [Multiline]
    public string missionDescription;
    [Tooltip("La Scene qui contient le niveau")]
    public string missionSceneName;
    [Space(10)]
    public Sprite buttonSkin;
    public Sprite textSkin;
}
