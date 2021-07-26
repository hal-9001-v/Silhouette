using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SolidFixer : MonoBehaviour
{

    Bounds _bounds;
    List<SolidNode> _nodes;
    // Possibilities of the Fixer
    void Awake()
    {
        _bounds = GetComponent<Collider>().bounds;
        _nodes = new List<SolidNode>();
    }

    private void FixedUpdate()
    {
        if (_nodes != null)
        {
            foreach (SolidNode node in _nodes)
            {
                Vector3 globalCoords = transform.TransformPoint(node.fixedPos);

                node.pos = globalCoords;
            }
        }

    }

    public void AddNode(SolidNode node)
    {
        if (_nodes != null)
        {
            _nodes.Add(node);
            node.fixedPos = transform.InverseTransformPoint(node.pos);

        }
    }

    public bool PointIsInside(Vector3 point)
    {
        if (_bounds == null)
            return false;

        return _bounds.Contains(point);
    }


}
