using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct WeightedVertex
{
    SolidNode a;
    SolidNode b;
    SolidNode c;
    SolidNode d;

    public float aWeight;
    public float bWeight;
    public float cWeight;
    public float dWeight;

    public bool isSet { get; private set; }

    public WeightedVertex(SolidNode a, float aWeight, SolidNode b, float bWeight, SolidNode c, float cWeight, SolidNode d, float dWeight)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;

        this.aWeight = aWeight;
        this.bWeight = bWeight;
        this.cWeight = cWeight;
        this.dWeight = dWeight;

        isSet = true;
    }

    public Vector3 GetPosition()
    {
        return a.pos * aWeight + b.pos * bWeight + c.pos * cWeight + d.pos * dWeight;
    }
}
