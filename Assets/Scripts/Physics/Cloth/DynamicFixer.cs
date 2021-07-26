using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DynamicFixer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Mesh _mesh;

    Bounds _bounds;
    
    List<FixNode> _nodes;

    // Possibilities of the Fixer
    void Awake()
    {
        _bounds = GetComponent<Collider>().bounds;
        _nodes = new List<FixNode>();


    }

    private void FixedUpdate()
    {
        if (_nodes != null)
        {
            foreach (FixNode node in _nodes)
            {
                Vector3 globalCoords = transform.TransformPoint(_mesh.vertices[node.vertexIndex]);

                node.clothNode.SetWorldPosition(globalCoords);
            }
        }

    }

    public void AddNode(ClothNode node)
    {
        if (_nodes != null)
        {
            _nodes.Add(new FixNode(node, GetClosestVertexIndex(transform.InverseTransformPoint(node.GetWorldPosition()))));
        }
    }

    int GetClosestVertexIndex(Vector3 position) {
        if (_mesh != null) {

            int closestVertex = 0;
            float closestDistance = float.MaxValue;

            float newDistance;

            for (int i = 1; i < _mesh.vertices.Length; i++) {
                newDistance = Vector3.Distance(position, _mesh.vertices[i]);

                if (newDistance < closestDistance) {
                    closestVertex = i;
                    closestDistance = newDistance;
                }
            }

            return closestVertex;
        }

        return -1;

    
    }

    public bool PointIsInside(Vector3 point)
    {
        if (_bounds == null)
            return false;

        return _bounds.Contains(point);
    }

    struct FixNode {
       public ClothNode clothNode;

       public int vertexIndex;

        public FixNode(ClothNode clothNode, int vertexIndex) {
            this.clothNode = clothNode;
            this.vertexIndex = vertexIndex;
        }
    }

}
