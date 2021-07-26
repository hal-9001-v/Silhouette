using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidNode
{
    public Vector3 pos;
    public Vector3 fixedPos;
    public Vector3 vel;
    public Vector3 force;

    public Vector3 normal;

    public float mass;
    int _massCounter;
    public bool isFixed;

    ElasticSolid _elasticSolid;

    public SolidNode(Vector3 startingPosition, ElasticSolid elasticSolid)
    {
        pos = startingPosition;

        vel = Vector3.zero;

        _elasticSolid = elasticSolid;

        mass = 0;
    }

    public void ComputeForces(float damping, Vector3 gravity, float penaltyFactor)
    {
        force += mass * gravity;

        //Damping
        force -= damping * vel;

        //Collision
        foreach (Collider collider in _elasticSolid.Colliders)
        {
            if (collider.bounds.Contains(pos))
            {
                Vector3 u = Vector3.zero;

                if (collider.GetType() == typeof(SphereCollider))
                {

                    u = GetSpherePenaltyDirection(collider);
                }
                else if (collider.GetType() == typeof(BoxCollider))
                {
                    u = GetBoxPenaltyDirection(collider);
                }
                else
                    if (collider.tag != "Plane")
                {
                    u = pos - collider.ClosestPoint(pos);
                }

                force += penaltyFactor * u;
            }
        }

        foreach (Collider plane in _elasticSolid.Planes)
        {
            if (plane.transform.InverseTransformPoint(pos).y < 0)
            {
                force += plane.transform.up * penaltyFactor;
            }
        }

    }

    public void AddTetrahedronMass(float newMass) {
        mass += newMass;
        _massCounter++;
    }

    public void SetAverageMass() {
        mass = mass / _massCounter;
    }

    Vector3 GetSpherePenaltyDirection(Collider collider)
    {
        return (pos - collider.bounds.center).normalized;
    }

    Vector3 GetBoxPenaltyDirection(Collider collider)
    {
        //Range: (-0.5f, 0.5f)

        Vector3 closesPoint = collider.ClosestPoint(pos);


        closesPoint = collider.transform.InverseTransformPoint(closesPoint);

        if (Mathf.Abs(closesPoint.x) > Mathf.Abs(closesPoint.y) && Mathf.Abs(closesPoint.x) > Mathf.Abs(closesPoint.z))
        {
            return collider.transform.right * Mathf.Sign(closesPoint.x);
        }
        else if (Mathf.Abs(closesPoint.y) > Mathf.Abs(closesPoint.z))
        {
            return collider.transform.up * Mathf.Sign(closesPoint.y);
        }
        else
        {
            return collider.transform.forward * Mathf.Sign(closesPoint.z);

        }
    }


}
