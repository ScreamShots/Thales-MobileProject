using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingManager : MonoBehaviour
{

    public MeshRenderer renderer_1;
    public MeshRenderer renderer_2;
    // Start is called before the first frame update

    [ContextMenu("SetLayers")]
    void Start()
    {
        renderer_1.sortingLayerID = SortingLayer.NameToID("AboveSea");
        renderer_1.sortingOrder = 0;
        renderer_2.sortingLayerID = SortingLayer.NameToID("Sea");
        renderer_2.sortingOrder = -1;


        Debug.Log(renderer_1.sortingLayerName + " " + renderer_1.sortingOrder);
        Debug.Log(renderer_2.sortingLayerName + " " + renderer_2.sortingOrder);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
