using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;

namespace PlayerEquipement
{    
    public class SonobuoyInstanceFeedback : MonoBehaviour
    {
        
        float scaleFactor
        {
            get
            {
                return (source.detectionRange * 2) / rangeRenderer.bounds.size.x;
            }
        }

        [Header("References")]

        [SerializeField]
        SonobuoyInstance source;

        [Header("Range Params")]

        [SerializeField]
        MeshRenderer rangeRenderer;
        [SerializeField]
        Material emptyRangeMaterial;
        [SerializeField]
        Material detectionRangeMaterial;


        public void Init()
        {
            rangeRenderer.transform.localScale *= scaleFactor;
        }

        public void DetectionRangeFeedBack(bool detection)
        {
            if (detection) rangeRenderer.material = detectionRangeMaterial;
            else rangeRenderer.material = emptyRangeMaterial;
        }
    }  
}

