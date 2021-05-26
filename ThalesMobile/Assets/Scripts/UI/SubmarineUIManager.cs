using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmarineUIManager : MonoBehaviour
{

    public Image fillBarBlocked;
    public Image fillBar;

    public Image submarineStatusImage;
    public Material submarineCalmStatusMaterial;
    public Material submarineAlertStatusMaterial;
    public Material submarinePanicStatusMaterial;
    public Material submarineNoise;

    public Canvas submarineCanvas;
    public GameObject nodeContainer;
    public GameObject nodeReference;

    public Sprite greenNode;

    private List<Image> nodes = new List<Image>();
    private int index;


    void Start()
    {
        submarineCanvas.worldCamera = GameManager.Instance.cameraController.cam;
        index = 0;
    }

    void Update()
    {
        
    }

    public void InitNodes(int count)
    {
        nodes.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(nodeReference, nodeContainer.transform);
            nodes.Add(go.GetComponent<Image>());
        }
    }

    public void LightNode()
    {                                              
        nodes[index].gameObject.GetComponent<Animator>().SetTrigger("Bounce");
        nodes[index].sprite = greenNode;
        index++;
    }
}
