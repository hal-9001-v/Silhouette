using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SolidEdge
{
    public int vertexA;
    public int vertexB;
    public float volume;

    public SolidEdge(int a, int b, float volume)
    {

        if (a < b)
        {
            vertexA = a;
            vertexB = b;
        }
        else
        {
            vertexA = b;
            vertexB = a;
        }

        this.volume = volume;
    }
}

class SolidEdgeEqualityComparer : IEqualityComparer<SolidEdge>
{
    public bool Equals(SolidEdge x, SolidEdge y)
    {
        if (x.vertexA == y.vertexA && x.vertexB == y.vertexB)
        {
            return true;
        }



        return false;
    }

    public int GetHashCode(SolidEdge obj)
    {
        int hcode = obj.vertexA * 100 + obj.vertexB * 10 - obj.vertexA % obj.vertexB;

        return hcode.GetHashCode();

    }
}

