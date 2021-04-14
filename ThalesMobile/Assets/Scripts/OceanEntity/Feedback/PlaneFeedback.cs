using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class PlaneFeedback : MonoBehaviour
    {
        [Header("Ship")]
        public Plane renderedPlane;

        [Header("UI")]
        public GameObject selectionCircle;

        // Update is called once per frame
        void Update()
        {   
            if(GameManager.Instance.playerController.currentSelectedEntity == renderedPlane)
            {
                selectionCircle.SetActive(true);
            }
            else
            {
                selectionCircle.SetActive(false);
            }     
        }
    }

}
