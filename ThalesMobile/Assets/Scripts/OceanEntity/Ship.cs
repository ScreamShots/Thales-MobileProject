using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class Ship : PlayerOceanEntity
    {
        private Transform _transform;

        [HideInInspector] public Transform currentTargetPoint;

        void Start()
        {
            movementType = MovementType.sea;
            _transform = transform;

            coords = new Coordinates(_transform.position, Vector2.up, 0);
        }

        void Update()
        {
            if (currentTargetPoint != null)
            {
                Move(Coordinates.ConvertWorldToVector2 (currentTargetPoint.position));
            }
        }
        public override void Move(Vector2 targetPosition)
        {
            //Calculate direction to target and store it in coords.
            coords.direction = targetPosition - coords.position;

            //Update the plane's position.
           coords.position += coords.direction.normalized * speed * Time.deltaTime;

            //Store the new position in the coords.
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

            if ((targetPosition - coords.position).magnitude < 0.1f)
            {
                currentTargetPoint = null;
            }
        }

        public override void PathFinding()
        {
            throw new System.NotImplementedException();
        }

        public override void Waiting()
        {
            throw new System.NotImplementedException();
        }
    }

}
