using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SolidNodeFace
{
    public SolidNode nodeA;
    public SolidNode nodeB;
    public SolidNode nodeC;

    public SolidNodeFace(SolidNode a, SolidNode b, SolidNode c)
    {
        nodeA = a;
        nodeB = b;
        nodeC = c;
    }

    public Vector3 GetNormal()
    {
        var u = nodeB.pos - nodeA.pos;
        var v = nodeC.pos - nodeA.pos;

        return Vector3.Cross(u, v).normalized;
    }

    public float GetArea()
    {
        //Heron's formula
        //area = (sqrt)(s * (s - a) * (s - b) * (s - c));
        //s = (a+b+c)/2

        float a = (nodeB.pos - nodeA.pos).magnitude;
        float b = (nodeC.pos - nodeB.pos).magnitude;
        float c = (nodeA.pos - nodeC.pos).magnitude;

        float s = a + b + c;
        s *= 0.5f;

        return Mathf.Pow((s * (s - a) * (s - b) * (s - c)), 0.5f);

    }

}
public class FaceEqualityComparer : IEqualityComparer<SolidNodeFace>
{
    public bool Equals(SolidNodeFace x, SolidNodeFace y)
    {
        if (x.nodeA == y.nodeA && x.nodeB == y.nodeB && x.nodeC == y.nodeC)
            return true;

        return false;

    }

    public int GetHashCode(SolidNodeFace obj)
    {
        return (int)(obj.nodeA.pos.magnitude * 101 + obj.nodeB.pos.magnitude * 10 + obj.nodeC.pos.magnitude);
    }
}
