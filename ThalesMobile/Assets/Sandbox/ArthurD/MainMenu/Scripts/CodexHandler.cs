using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CodexHandler : MonoBehaviour
{
    public GameObject categoriePrefab;
    public GameObject buttonPrefab;

    public float offSetBtwButton;
    public float offSetBtwCat;

    public RectTransform content;

    [ReorderableList]
    public List<GameObject> ElementToPlace;

    [ContextMenu("Place")]
    public void PlaceElement()
    {
        //Clear
        if(content.childCount != 0)
        {
            GameObject[] gos = content.gameObject.GetChildren(false);
            foreach (var go in gos)
            {
                DestroyImmediate(go);
            }
        }

        float offsetPos = 0;

        for (int i = 0; i < ElementToPlace.Count; i++)
        {
            offsetPos += offSetBtwButton;

            RectTransform t =  Instantiate(ElementToPlace[i], content.position, Quaternion.identity, content).GetComponent<RectTransform>();
            t.position = content.position + Vector3.down * offSetBtwButton;
        }

    }


}
