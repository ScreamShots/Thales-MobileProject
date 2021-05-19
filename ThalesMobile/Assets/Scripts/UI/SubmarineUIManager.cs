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

    public Canvas submarineCanvas;
    public GameObject nodeContainer;
    public GameObject nodeReference;

    public Sprite greenNode;

    private List<Image> nodes = new List<Image>();


    void Start()
    {
        submarineCanvas.worldCamera = GameManager.Instance.cameraController.cam;

        InitNodes(3);//do this in submarine
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

    public void LightNode(int index)
    {
        nodes[index].sprite = greenNode;
    }
}
