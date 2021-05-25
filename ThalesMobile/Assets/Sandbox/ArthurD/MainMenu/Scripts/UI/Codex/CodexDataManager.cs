using UnityEngine.UI;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class CodexDataManager : MonoBehaviour
{
    [Header("Component")]
    public GameObject descriptionCanvas;
    public TextMeshProUGUI prodTitle;
    [Space(10)]
    public Image spriteField;
    public MultiTextSystem multiText;
    public TextMeshProUGUI[] codexButtonTitle;

    [Header("Parameter")]
    public CodexSCO codex;

    [ContextMenu("UpdateButton"), Button("UpdateButton")]
    private void Start()
    {
        if (codexButtonTitle.Length != 0)
        {
            for (int i = 0; i < codexButtonTitle.Length; i++)
            {
                codexButtonTitle[i].text = codex.data[i].title;
                codexButtonTitle[i].gameObject.name = "Titre:" + codex.data[i].title;
                codexButtonTitle[i].gameObject.GetParent().name = "Button_" + codex.data[i].title;
            }
        }
    }

    public void LoadDescription(int number)
    {
        spriteField.sprite = codex.data[number].categoIcon;
        prodTitle.text = codex.data[number].title;
        multiText.data = codex.data[number];

        descriptionCanvas.SetActive(true);
    }
}
