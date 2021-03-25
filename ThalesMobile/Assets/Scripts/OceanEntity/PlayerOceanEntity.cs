using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public abstract class PlayerOceanEntity : OceanEntity
    {
        public float speed;
        public float acceleration;
        public MovementType movementType;

        /// <summary>
        /// Use this method to create the waiting routine of the entity
        /// </summary>
        public abstract void Waiting(); 
    }

}
