using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MissionLoader : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI missionDescription;
    [Space(10)]
    public Button lauchMissionButton;
    [Space(10)]
    public Image buttonSkin;
    public Image textSkin;

    [Header("Parameter")]
    public GlobePoint loadedMission;

    public void LoadMission(GlobePoint mission)
    {
        loadedMission = mission;

        //Change Mission description
        missionDescription.text = mission.missionDescription;

        //Change Mission Visual
        buttonSkin.sprite = mission.buttonSkin;
        textSkin.sprite = mission.textSkin;
    }

    public void StartMission()
    {
        SceneManager.LoadScene(loadedMission.missionSceneName);
    }
}
