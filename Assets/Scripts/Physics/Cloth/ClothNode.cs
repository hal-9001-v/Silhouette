using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothNode
{
    public Vector3 pos;
    public Vector3 fixedPos;
    public Vector3 vel;
    public Vector3 force;

    public float mass;
    public bool isFixed;

    MassSpringCloth _massSpringCloth;

    public ClothNode(Vector3 startingPosition, float mass, MassSpringCloth massSpringCloth)
    {
        pos = startingPosition;

        vel = Vector3.zero;
        this.mass = mass;

        _massSpringCloth = massSpringCloth;

    }

    public void ComputeForces(float damping, float penaltyFactor)
    {
        if (_massSpringCloth != null)
        {
            force += mass * _massSpringCloth.localGravity;

            //Damping
            force -= damping * vel;

            //Collision
            foreach (Collider collider in _massSpringCloth.Colliders)
            {
                var worldPosition = GetWorldPosition();
                if (collider.bounds.Contains(worldPosition))
                {
                    var u = worldPosition - collider.ClosestPoint(worldPosition);

                    /*
                    if (collider.attachedRigidbody != null)
                    {
                        collider.attachedRigidbody.AddForce(-penaltyFactor * u);
                    }
                    */

                    u = _massSpringCloth.transform.InverseTransformDirection(u);

                    force += penaltyFactor * u;
                }

            }

        }

    }

    public Vector3 GetWorldPosition()
    {
        if (_massSpringCloth != null)
        {
            return _massSpringCloth.transform.TransformPoint(pos);
        }

        return Vector3.zero;

    }

    public void SetWorldPosition(Vector3 newPosition)
    {
        pos = _massSpringCloth.transform.InverseTransformPoint(newPosition);
    }

}
