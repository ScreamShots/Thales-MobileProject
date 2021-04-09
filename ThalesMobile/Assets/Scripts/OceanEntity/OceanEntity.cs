using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OceanEntities
{
    /// <summary>
    /// Thomas Depraz - 25/03/2021 - Sea Level is an enum describing the entity's sea level, meanig the environment in which they evolve.
    /// </summary>
    public enum SeaLevel
    {
        sea,
        submarine,
        air
    }

    /// <summary>
    /// Thomas Depraz - 25/03/2021 - OceanEntity is the root class for ever other entity.
    /// </summary>
    public abstract class OceanEntity : MonoBehaviour
    {
        public Coordinates coords;
        public SeaLevel currentSeaLevel;
        //public Zone currentZone; -- the Zone class isn't created yet

        /// <summary>
        /// Use this method to make the entity move and pass the new informations to coords
        /// </summary>
        /// <param name="targetPosition">Where is the entity headed</param>
        public abstract void Move(Vector2 targetPosition);

        public abstract void PathFinding(); // A* implementation placeholder for Antoine Grugeon -- the method doesn't have to be abstract
    }
}

