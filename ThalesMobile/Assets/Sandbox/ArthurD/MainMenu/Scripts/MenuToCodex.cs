using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

[ExecuteAlways]
public class MenuToCodex : MonoBehaviour
{
    [ReadOnly] public Vector2 screenSize;
    [ReadOnly] public Vector3 centerPos;

    public GlobeInput input = null;

    [Header("Silder and screen")]
    public Slider slider;
    [Space(10)]
    public RectTransform menu;
    public RectTransform codex;

    [Header("Button Visual")]
    public Image sliderImage;
    [Space(10)]
    public Sprite ToCodex;
    public Sprite ToMenu;

    void Update()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        centerPos = new Vector3(screenSize.x * 0.5f, screenSize.y * 0.5f);

        menu.sizeDelta = screenSize;
        codex.sizeDelta = screenSize;

        //Si je ne touche pas l'écran
        if (!input.isDraging)
        {
            if (slider.value > 0.5f)
            {
                slider.value = Mathf.Lerp(slider.value, 1, 0.1f);
            }
            else
            {
                slider.value = Mathf.Lerp(slider.value, 0, 0.1f);
            }
        }

        sliderImage.sprite = slider.value > 0.5f ? ToCodex : ToMenu;

        menu.position = centerPos;// + new Vector3(-centerPos.x * 2 * slider.value, 0);
        codex.position = centerPos + new Vector3(centerPos.x * 2 * (1 - slider.value), 0);
    }
}
