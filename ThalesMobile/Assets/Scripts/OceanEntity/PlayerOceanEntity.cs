using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweek.FlagAttributes;


namespace OceanEntities
{     
    public enum MovementType
    {
        air,
        sea
    }

    /// <summary>
    /// Thomas Depraz - 25/03/2021 -  PlayerOceanEntity is the root class for every player controlled player OceanEntities
    /// </summary>
    
    [TweekClass]
    public abstract class PlayerOceanEntity : OceanEntity
    {
        [TweekFlag(FieldUsage.Gameplay)]
        public float speed;
        [TweekFlag(FieldUsage.Gameplay)]
        public float acceleration;
        [TweekFlag(FieldUsage.Gameplay)]
        public float rotateSpeed;
        public MovementType movementType;
        public Vector2 currentTargetPoint = new Vector2(-9999,-9999);
        [HideInInspector]public Vector2 nullVector = new Vector2(-9999, -9999);

        [Header("Feedback")]
        public PlayerOceanEntityFeedback enitityFeedback;

        [Header("UI")]
        public GameObject entityDeck;
        [HideInInspector]public EntitiesSelectionButton linkedButton;

        [HideInInspector] public GameObject equipementFeedback;


        /// <summary>
        /// Use this method to create the waiting routine of the entity
        /// </summary>
        public abstract void Waiting(); 
    }

}
