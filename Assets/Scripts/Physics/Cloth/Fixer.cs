using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Fixer : MonoBehaviour
{

    Bounds _bounds;
    List<ClothNode> _nodes;
    // Possibilities of the Fixer
    void Awake()
    {
        _bounds = GetComponent<Collider>().bounds;
        _nodes = new List<ClothNode>();
    }

    private void FixedUpdate()
    {
        if (_nodes != null)
        {
            foreach (ClothNode node in _nodes)
            {
                Vector3 globalCoords = transform.TransformPoint(node.fixedPos);

                node.SetWorldPosition(globalCoords);
            }
        }

    }

    public void AddNode(ClothNode node)
    {
        if (_nodes != null)
        {
            _nodes.Add(node);
            node.fixedPos = transform.InverseTransformPoint(node.GetWorldPosition());
        }
    }

    public bool PointIsInside(Vector3 point)
    {
        if (_bounds == null)
            return false;

        return _bounds.Contains(point);
    }


}
