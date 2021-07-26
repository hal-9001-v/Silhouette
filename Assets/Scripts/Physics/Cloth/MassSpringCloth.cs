using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MassSpringCloth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MeshFilter _meshFilter;

    [Header("Settings")]
    [SerializeField] bool _paused;
    [SerializeField] Integration _integrationMethod;
    [SerializeField] [Range(0.001f, 0.05f)] float _stepTime = 0.01f;
    [SerializeField] [Range(0.1f, 5)] float _localFixedScale = 1;
    float stepElapsedTime = 0;

    [SerializeField] LayerMask _collisionLayers;
    [SerializeField] LayerMask _windColliderLayers;

    [Space(3)]
    [Header("Nodes")]
    [SerializeField] [Range(0.1f, 100)] float _mass = 1;
    [SerializeField] [Range(0.0f, 1)] float _nodeDamping = 0.1f;
    [SerializeField] [Range(0.0f, 100)] float _penaltyFactor = 0.1f;

    [Header("Springs")]
    [SerializeField] [Range(0.1f, 500)] float _tractionStiffness = 1;
    [SerializeField] [Range(0.1f, 500)] float _bendingStiffness = 1;
    [SerializeField] [Range(0.0f, 1)] float _springDamping = 0.1f;

    [Header("Forces")]
    [SerializeField] Vector3 _gravity;
    [SerializeField] Vector3 _windVelocity;
    [SerializeField] [Range(0.1f, 10)] float _windChangeSpeed = 1;
    float windElapsedTime;

    public Vector3 localGravity { get; private set; }

    [Header("Gizmos")]
    [SerializeField] bool _drawNodes;
    [SerializeField] [Range(0f, 1f)] float _nodeRadius = 0.1f;
    [SerializeField] Color _nodeColor = Color.blue;

    [SerializeField] bool _drawNodeIndex;
    [SerializeField] Vector3 _indexOffset;


    [Space(10)]
    [SerializeField] bool _drawSprings;
    [SerializeField] Color _springColor = Color.red;

    [Space(10)]
    [SerializeField] bool _drawFlexSprings;
    [SerializeField] Color _flexSpringColor = Color.yellow;

    //Objects
    ClothNode[] _nodes;
    NodeTriangle[] _nodeTriangles;

    List<ClothSpring> _tractionSprings;
    List<ClothSpring> _bendingSprings;

    Mesh _mesh;

    public List<Collider> Colliders { get; private set; }

    List<WindCollider> _windColliders;


    private void Start()
    {
        if (_meshFilter != null && _meshFilter.mesh != null)
        {
            _mesh = _meshFilter.mesh;

            #region Enviroment
            Colliders = new List<Collider>();
            foreach (Collider collider in FindObjectsOfType<Collider>())
            {

                if (_collisionLayers == (_collisionLayers | (1 << collider.gameObject.layer)))
                {
                    Colliders.Add(collider);

                }
            }
            

            _windColliders = new List<WindCollider>();
            foreach (WindCollider windCollider in FindObjectsOfType<WindCollider>())
            {
                if (_windColliderLayers == (_windColliderLayers | (1 << windCollider.gameObject.layer)))
                {
                    _windColliders.Add(windCollider);
                }

            }


            #endregion

            CreateNodes();
            CreateNodeTriangles();
            CreateSprings();
        }
        else
        {
            Debug.LogWarning("No mesh in MassSpringCloth " + name);
        }
    }

    void CreateNodes()
    {

        Fixer[] fixers = FindObjectsOfType<Fixer>();
        if (_mesh != null)
        {
            _nodes = new ClothNode[_mesh.vertices.Length];

            for (int i = 0; i < _nodes.Length; i++)
            {
                _nodes[i] = new ClothNode(_mesh.vertices[i], _mass, this);


                foreach (Fixer fixer in fixers)
                {
                    if (fixer.PointIsInside(transform.TransformPoint(_mesh.vertices[i])))
                    {
                        _nodes[i].isFixed = true;
                        fixer.AddNode(_nodes[i]);
                    }
                }
            }


        }
    }

    void CreateNodeTriangles()
    {
        if (_mesh != null && _nodes != null)
        {
            int[] meshTriangles = _mesh.triangles;
            _nodeTriangles = new NodeTriangle[_mesh.triangles.Length / 3];

            for (int i = 0; i < _nodeTriangles.Length; i++)
            {
                _nodeTriangles[i] = new NodeTriangle();

                _nodeTriangles[i].nodeA = _nodes[meshTriangles[i * 3]];
                _nodeTriangles[i].nodeB = _nodes[meshTriangles[i * 3 + 1]];
                _nodeTriangles[i].nodeC = _nodes[meshTriangles[i * 3 + 2]];

            }

        }
    }

    void CreateSprings()
    {
        if (_mesh != null)
        {
            _tractionSprings = new List<ClothSpring>();
            _bendingSprings = new List<ClothSpring>();

            EdgeEqualityComparer edgeComparer = new EdgeEqualityComparer();
            Dictionary<Edge, Edge> edgeDictionary = new Dictionary<Edge, Edge>(edgeComparer);

            Edge[] triangleEdges = new Edge[3];
            for (int i = 0; i < _mesh.triangles.Length; i += 3)
            {
                int firstVertexIndex = _mesh.triangles[i];
                int secondVertexIndex = _mesh.triangles[i + 1];
                int thirdVertexIndex = _mesh.triangles[i + 2];

                triangleEdges[0] = new Edge(firstVertexIndex, secondVertexIndex, thirdVertexIndex);
                triangleEdges[1] = new Edge(secondVertexIndex, thirdVertexIndex, firstVertexIndex);
                triangleEdges[2] = new Edge(thirdVertexIndex, firstVertexIndex, secondVertexIndex);



                Edge otherEdge;
                foreach (Edge edge in triangleEdges)
                {
                    if (!edgeDictionary.TryGetValue(edge, out otherEdge))
                    {
                        edgeDictionary.Add(edge, edge);

                        _tractionSprings.Add(new ClothSpring(_nodes[edge.vertexA], _nodes[edge.vertexB]));
                    }
                    else
                    {
                        _bendingSprings.Add(new ClothSpring(_nodes[edge.vertexC], _nodes[otherEdge.vertexC]));
                    }

                }

            }
        }

        Debug.Log("<color=yellow>Cloth " + name + ":</color> Traction Springs: " + _tractionSprings.Count + ", Bending Springs: " + _bendingSprings.Count);
    }

    public MassSpringCloth()
    {
        _paused = true;
        _gravity = new Vector3(0.0f, -9.81f, 0.0f);
        _integrationMethod = Integration.Symplectic;
    }

    public enum Integration
    {
        Explicit = 0,
        Symplectic = 1,
    }

    public void FixedUpdate()
    {
        if (_paused)
            return; // Not simulating

        while (stepElapsedTime > Time.fixedDeltaTime * _localFixedScale)
        {
            stepElapsedTime -= Time.fixedDeltaTime * _localFixedScale;

            ComputeForces();

            // Select integration method
            switch (_integrationMethod)
            {
                case Integration.Explicit:
                    StepExplicit();
                    break;

                case Integration.Symplectic:
                    StepSymplectic();
                    break;

                default:
                    throw new System.Exception("[ERROR] Should never happen!");
            }

        }

        stepElapsedTime += Time.deltaTime;


    }


    void ComputeForces()
    {
        if (_nodes != null && _tractionSprings != null && _nodeTriangles != null)
        {

            localGravity = transform.InverseTransformDirection(_gravity);

            foreach (ClothNode node in _nodes)
            {
                node.force = Vector3.zero;
                node.ComputeForces(_nodeDamping, _penaltyFactor);
            }

            ComputeWindForce();

            foreach (ClothSpring spring in _tractionSprings)
            {
                spring.ComputeForces(_tractionStiffness, _springDamping);
            }

            foreach (ClothSpring spring in _bendingSprings)
            {
                spring.ComputeForces(_bendingStiffness, _springDamping);
            }
        }
    }

    void StepExplicit()
    {
        if (_nodes != null)
        {
            foreach (ClothNode node in _nodes)
            {
                if (!node.isFixed)
                {
                    node.pos += _stepTime * node.vel;
                    node.vel += _stepTime / node.mass * node.force;

                }
            }


            UpdateVertices();
        }


    }

    private void StepSymplectic()
    {
        if (_nodes != null)
        {
            foreach (ClothNode node in _nodes)
            {
                if (!node.isFixed)
                {
                    node.vel += _stepTime / node.mass * node.force;
                    node.pos += _stepTime * node.vel;
                }
            }


            UpdateVertices();
        }
    }
 
    void ComputeWindForce()
    {


        float windTimeFactor = Mathf.Abs(Mathf.Sin(windElapsedTime));

        windElapsedTime += _stepTime * _windChangeSpeed;

        foreach (NodeTriangle triangle in _nodeTriangles)
        {
            Vector3 normal = triangle.GetNormal();

            Vector3 triangleVelocity = triangle.nodeA.vel + triangle.nodeB.vel + triangle.nodeC.vel;
            triangleVelocity = triangleVelocity / 3;

            Vector3 windColliderVelocity = Vector3.zero;
            foreach (WindCollider collider in _windColliders)
            {
                if (collider.PointIsColliding(triangle.nodeA.GetWorldPosition())
                    || collider.PointIsColliding(triangle.nodeB.GetWorldPosition())
                    || collider.PointIsColliding(triangle.nodeC.GetWorldPosition()))
                {
                    windColliderVelocity += collider.windVelocity;
                }
            }


            Vector3 localWindVelocity = transform.InverseTransformVector(_windVelocity + windColliderVelocity);

            Vector3 windForce = windTimeFactor * triangle.GetArea() * (Vector3.Dot(normal, localWindVelocity - triangleVelocity)) * normal;

            windForce = windForce / 3;
            triangle.nodeA.force += windForce;
            triangle.nodeB.force += windForce;
            triangle.nodeC.force += windForce;

        }
    }

    private void UpdateVertices()
    {
        //Procedure to update vertex positions
        Vector3[] vertices = new Vector3[_mesh.vertexCount];

        for (int i = 0; i < _nodes.Length; i++)
        {
            vertices[i] = _nodes[i].pos;
        }

        _mesh.vertices = vertices;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_drawNodes && _nodes != null)
        {
            for (int i = 0; i < _nodes.Length; i++)
            {
                {
                    Gizmos.color = _nodeColor;
                    Gizmos.DrawSphere(transform.TransformPoint(_nodes[i].pos), _nodeRadius);

                    if (_drawNodeIndex)
                        Handles.Label(transform.TransformPoint(_nodes[i].pos) + _indexOffset, "Node " + i);
                }
            }

        }

        if (_drawSprings && _tractionSprings != null)
        {
            foreach (ClothSpring spring in _tractionSprings)
            {
                Gizmos.color = _springColor;
                Gizmos.DrawLine(transform.TransformPoint(spring.nodeA.pos), transform.TransformPoint(spring.nodeB.pos));
            }
        }

        if (_drawFlexSprings && _bendingSprings != null)
        {
            foreach (ClothSpring spring in _bendingSprings)
            {
                Gizmos.color = _flexSpringColor;
                Gizmos.DrawLine(transform.TransformPoint(spring.nodeA.pos), transform.TransformPoint(spring.nodeB.pos));
            }
        }

    }
#endif

    struct Edge
    {
        public int vertexA;
        public int vertexB;
        public int vertexC;

        public Edge(int a, int b, int c)
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

            vertexC = c;
        }
    }

    class EdgeEqualityComparer : IEqualityComparer<Edge>
    {
        public bool Equals(Edge x, Edge y)
        {
            if (x.vertexA == y.vertexA && x.vertexB == y.vertexB)
            {
                return true;
            }



            return false;
        }

        public int GetHashCode(Edge obj)
        {
            int hcode = obj.vertexA * 100 + obj.vertexB * 10 - obj.vertexA % obj.vertexB;

            return hcode.GetHashCode();

        }
    }

    struct NodeTriangle
    {
        public ClothNode nodeA;
        public ClothNode nodeB;
        public ClothNode nodeC;

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
}
