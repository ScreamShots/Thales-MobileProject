﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
        public Environnement environment;
        public ZoneState currentZoneState;
        public Zone currentZone;

        //public Zone currentZone; -- the Zone class isn't created yet
        [Header("Pathfinding settings")]
        public Seeker seeker;
        public float pathUpdatingFrequency;
        private Path path;
        private int currentWaypoint;
        private float timeBeforeNextPathUpdate;
        protected bool pathEndReached;
        protected Vector2 pathDirection;
        public float nextWaypointDistance;
        public int waypointAhead;
        [HideInInspector] public Vector2 pathDestination;



        /// <summary>
        /// Use this method to make the entity move and pass the new informations to coords
        /// </summary>
        /// <param name="targetPosition">Where is the entity headed</param>
        public abstract void Move(Vector2 targetPosition);

        private void CalculatePath()
        {
            seeker.StartPath(Coordinates.ConvertVector2ToWorld(coords.position), Coordinates.ConvertVector2ToWorld(pathDestination), OnPathComplete);
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }
        /// <summary>
        /// Call this method in the Update if the entity needs pathfinding, the result is stored in : "pathDirection"
        /// </summary>
        protected void UpdatePath()
        {
            if (timeBeforeNextPathUpdate <= 0)
            {
                timeBeforeNextPathUpdate = pathUpdatingFrequency;

                CalculatePath();

                if (path != null)
                {
                    if (currentWaypoint >= path.vectorPath.Count)
                    {
                        pathEndReached = true;
                    }
                    else
                    {
                        pathEndReached = false;

                        while (Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance && path.vectorPath.Count > currentWaypoint + 1)
                        {
                            currentWaypoint++;
                        }

                        pathDirection = (Coordinates.ConvertWorldToVector2(path.vectorPath[currentWaypoint + (((currentWaypoint + waypointAhead) < path.vectorPath.Count) ? waypointAhead : 0)] - transform.position)).normalized;
                    }
                }
            }

            if (timeBeforeNextPathUpdate > 0)
            {
                timeBeforeNextPathUpdate -= Time.deltaTime;
            }
        }
    }
}

