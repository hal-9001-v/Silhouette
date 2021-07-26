using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothSpring
{
    public ClothNode nodeA { get; private set; }
    public ClothNode nodeB { get; private set; }

    float _startLength;

    public ClothSpring(ClothNode a, ClothNode b)
    {
        nodeA = a;
        nodeB = b;

        _startLength = GetLengthBetweenNodes();
    }

    public void ComputeForces(float stiffness, float damping)
    {
        Vector3 u = nodeA.pos - nodeB.pos;
        u.Normalize();

        float currentLength = GetLengthBetweenNodes();

        Vector3 force = -stiffness * (currentLength - _startLength) * u;

        force -= damping * (nodeA.vel - nodeB.vel);

        force -= damping * (Vector3.Dot(u, (nodeA.vel - nodeB.vel))) * u;

        nodeA.force += force;
        nodeB.force -= force;


    }

    float GetLengthBetweenNodes()
    {
        return (nodeA.pos - nodeB.pos).magnitude;
    }
}
