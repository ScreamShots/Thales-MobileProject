using System.Collections;
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
        protected float timeBeforeNextPathUpdate;
        protected bool pathEndReached;
        protected Vector2 pathDirection;
        private Vector2 lastValidPos;
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
        protected Vector2 UpdatePath(Vector2 target)
        {
            pathDestination = target;

            CalculatePath();
            if(path!= null)
            {
                if (timeBeforeNextPathUpdate <= 0 && path.IsDone())
                {
                    lastValidPos = pathDestination;

                    RaycastHit hit;
                    timeBeforeNextPathUpdate = pathUpdatingFrequency;
                    Vector3 destDirection = (Coordinates.ConvertVector2ToWorld(pathDestination)) - (Coordinates.ConvertVector2ToWorld(coords.position));
                    float destDistance = destDirection.magnitude;
                    destDirection.Normalize();
                    if (!Physics.SphereCast(Coordinates.ConvertVector2ToWorld(coords.position) + Vector3.up * 0.5f, 0.6f,destDirection, out hit, destDistance, LayerMask.GetMask("LandPathfinding")))
                    {
                        pathDirection = pathDestination - coords.position;
                        pathDirection.Normalize();
                    }
                    else
                    {
                        Debug.DrawRay(hit.point, Vector3.up, Color.red, 0.5f);

                    

                        if (path != null)
                        {
                            if (environment.ZoneIn(pathDestination) != 0
                                && environment.zones[environment.ZoneIn(pathDestination) - 1].state == ZoneState.LandCoast)
                            {
                            }
                            lastValidPos = Coordinates.ConvertWorldToVector2(path.vectorPath[path.vectorPath.Count - 1]);

                            if (currentWaypoint >= path.vectorPath.Count)
                            {
                                pathEndReached = true;
                            }
                            else
                            {
                                pathEndReached = false;

                                while (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance && path.vectorPath.Count > currentWaypoint + 1)
                                {
                                    currentWaypoint++;
                                }

                                pathDirection = (Coordinates.ConvertWorldToVector2(path.vectorPath[currentWaypoint + (((currentWaypoint + waypointAhead) < path.vectorPath.Count) ? waypointAhead : 0)] - transform.position)).normalized;
                            }
                        }
                    }
                }
            }

            if (timeBeforeNextPathUpdate > 0)
            {
                timeBeforeNextPathUpdate -= Time.deltaTime;
            }
            return lastValidPos;
        }
    }
}

