using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidSpring
{
    public SolidNode nodeA { get; private set; }
    public SolidNode nodeB { get; private set; }

    float volume;
    int _volumeCounter;

    float _startLength;

    //Force = -V/(startLength^2) * density * (Length- startLength) * (a.pos - b.pos)/Length => -V/(startLegth ^ 2) is constant, k may vary on runtime
    float _stiffnessConstant;

    public SolidSpring(SolidNode a, SolidNode b)
    {
        nodeA = a;
        nodeB = b;

        _startLength = GetLengthBetweenNodes();
    }

    public void AddTetrahedronVolume(float v)
    {
        if (v > 0)
        {
            volume += v;
            _volumeCounter++;
        }

    }

    public void SetAverageVolume()
    {
        volume = volume / _volumeCounter;

        _stiffnessConstant = -volume / (_startLength * _startLength);
    }

    public void ComputeForces(float density, float damping, float stiffnessFactor)
    {
        Vector3 u = nodeA.pos - nodeB.pos;
        u.Normalize();

        float currentLength = GetLengthBetweenNodes();

        //Force =  -V/(startLength^2)    * density * (Length- startLength) * (a.pos - b.pos)/Length => -V/(startLegth ^ 2)
        Vector3 force = stiffnessFactor * _stiffnessConstant * density * (currentLength - _startLength) * u;

        force -= damping * (nodeA.vel - nodeB.vel);

        //force -= damping * (Vector3.Dot(u, (nodeA.vel - nodeB.vel))) * u;

        nodeA.force += force;
        nodeB.force -= force;


    }

    float GetLengthBetweenNodes()
    {
        return (nodeA.pos - nodeB.pos).magnitude;
    }
}
