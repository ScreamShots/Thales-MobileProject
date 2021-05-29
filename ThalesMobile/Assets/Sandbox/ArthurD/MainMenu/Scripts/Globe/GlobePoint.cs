using UnityEngine;
using NaughtyAttributes;
public enum MissionDifficulty { Easy, Medium, Hard };

[System.Serializable]
public class GlobePoint
{
    [Header("Mision Data")]
    [TextArea(1, 2)] public string missionTitle;
    [TextArea(1, 1)] public string missionEmplacement;
    [ResizableTextArea]
    public string missionDescription;

    [Header("Globe Info")]
    public Vector2 pointCoord = Vector2.zero;
    [Tooltip("Niveau de difficulté")]
    public MissionDifficulty missionDifficulty = MissionDifficulty.Easy;
    [Space(5)]
    [Tooltip("La Scene qui contient le niveau"), Scene]
    public int missionSceneIndex;

    [Header("Globe Visual")]
    public GameObject button;
    public RectTransform buttonrectTrans;
}
