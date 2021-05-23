using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MissionLoader : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI missionTitle;
    public TextMeshProUGUI missionEmplacement;
    public TextMeshProUGUI missionDescription;
    [Space(10)]
    public Image briefingSkin;
    public Image missionDescriptionSkin;
    public Image lauchButtonSkin;
    [Space(10)]
    public Button missionLauchButton;

    [Header("Parameter")]
    public GlobePoint loadedMission;

    [Header("Easy")]
    public GameObject difficultyEasy;
    public Sprite briefingSkinEasy;
    public Sprite descriptionSkinEasy;
    public Sprite lauchButtonSkinEasy;

    [Header("Medium")]
    public GameObject difficultyMedium;
    [Space(10)]
    public Sprite briefingSkinMedium;
    public Sprite descriptionSkinMedium;
    public Sprite lauchButtonSkinMedium;

    [Header("Hard")]
    public GameObject difficultyHard;
    [Space(10)]
    public Sprite briefingSkinHard;
    public Sprite descriptionSkinHard;
    public Sprite lauchButtonSkinHard;


    public void LoadMission(GlobePoint mission)
    {
        loadedMission = mission;

        //Show the difficulty
        switch (mission.missionDifficulty)
        {
            case MissionDifficulty.Easy:
                difficultyEasy.SetActive(true);
                difficultyMedium.SetActive(false);
                difficultyHard.SetActive(false);
                break;

            case MissionDifficulty.Medium:
                difficultyEasy.SetActive(false);
                difficultyMedium.SetActive(true);
                difficultyHard.SetActive(false);
                break;

            case MissionDifficulty.Hard:
                difficultyEasy.SetActive(false);
                difficultyMedium.SetActive(false);
                difficultyHard.SetActive(true);
                break;

            default:
                difficultyEasy.SetActive(false);
                difficultyMedium.SetActive(false);
                difficultyHard.SetActive(false);
                break;
        }

        //Change the Skin
        switch (mission.missionDifficulty)
        {
            case MissionDifficulty.Easy:
                briefingSkin.sprite = briefingSkinEasy;
                missionDescriptionSkin.sprite = descriptionSkinEasy;
                lauchButtonSkin.sprite = lauchButtonSkinEasy;
                break;

            case MissionDifficulty.Medium:
                briefingSkin.sprite = briefingSkinMedium;
                missionDescriptionSkin.sprite = descriptionSkinMedium;
                lauchButtonSkin.sprite = lauchButtonSkinMedium;
                break;

            case MissionDifficulty.Hard:
                briefingSkin.sprite = briefingSkinHard;
                missionDescriptionSkin.sprite = descriptionSkinHard;
                lauchButtonSkin.sprite = lauchButtonSkinHard;
                break;

            default:
                briefingSkin.sprite = briefingSkinHard;
                missionDescriptionSkin.sprite = descriptionSkinHard;
                lauchButtonSkin.sprite = lauchButtonSkinHard;
                break;
        }

        //Change Mission text
        missionTitle.text = mission.missionTitle;
        missionEmplacement.text = mission.missionEmplacement;
        missionDescription.text = mission.missionDescription;
    }

    public void StartMission()
    {
        GameManager.Instance.sceneHandler.LoadScene(1);
        //SceneManager.LoadScene(loadedMission.missionSceneName);
    }
}
