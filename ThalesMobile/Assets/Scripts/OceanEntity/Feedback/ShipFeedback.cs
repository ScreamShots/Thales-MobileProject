using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class ShipFeedback : MonoBehaviour
    {
        [Header("Ship")]
        public Ship renderedShip;

        [Header("UI")]
        public GameObject selectionCircle;

        // Update is called once per frame
        void Update()
        {
            
            if(GameManager.Instance.playerController.currentSelectedEntity == renderedShip)
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

