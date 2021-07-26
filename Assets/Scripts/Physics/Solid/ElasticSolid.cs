using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

public class ElasticSolid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MeshFilter _meshFilter;
    [SerializeField] TextAsset _nodesFile;
    [SerializeField] TextAsset _eleFile;

    [Header("Settings")]
    [SerializeField] bool _paused = true;
    [SerializeField] Integration _integrationMethod;
    [SerializeField] [Range(0.001f, 0.05f)] float _stepTime = 0.00001f;
    [SerializeField] [Range(0.1f, 5)] float _localFixedScale = 1;
    float stepElapsedTime = 0;

    [SerializeField] Vector3 _nodeMeshOffset = new Vector3(0, 0, 0);
    [SerializeField] [Range(0, 2)] float _nodeMeshScale = 1;
    [SerializeField] Vector3 _nodeMeshRotation = new Vector3(0, 0, 0);

    [SerializeField] LayerMask _collisionLayers;

    [Space(3)]
    [Header("Nodes")]
    [SerializeField] [Range(0.1f, 10000)] float _density = 1;
    [SerializeField] [Range(0.0f, 1)] float _nodeDamping = 0.1f;
    [SerializeField] [Range(0.0f, 100)] float _penaltyFactor = 0.1f;

    [Header("Springs")]
    [SerializeField] [Range(0.0f, 300)] float _springStiffness = 0.1f;
    [SerializeField] [Range(0.0f, 1)] float _springDamping = 0.1f;

    [Header("Forces")]
    [SerializeField] Vector3 _gravity;
    [SerializeField] Vector3 _windVelocity;
    [SerializeField] [Range(0.1f, 10)] float _windChangeSpeed = 1;
    float windElapsedTime;

    [Header("Gizmos")]
    [SerializeField] bool _drawNodes;
    [SerializeField] [Range(0f, 1f)] float _nodeRadius = 0.1f;
    [SerializeField] Color _nodeColor = Color.blue;
    //[SerializeField] Color _innerNodeColor = Color.yellow;

    [SerializeField] bool _drawNodeIndex;
    [SerializeField] Vector3 _indexOffset;

    [Space(10)]
    [SerializeField] bool _drawSprings;
    [SerializeField] Color _springColor = Color.red;

    //Objects
    List<SolidNode> _nodes;
    List<SolidNodeFace> _faces;
    WeightedVertex[] _weightedVertices;
    List<SolidSpring> _springs;

    WindCollider[] _windColliders;
    public List<Collider> Planes { get; private set; }
    public List<Collider> Colliders { get; private set; }
    Mesh _mesh;

    public enum Integration
    {
        Symplectic = 0,
        Explicit = 1
    }
    private void Start()
    {
        if (_meshFilter != null && _meshFilter.mesh != null && _nodesFile != null)
        {
            _mesh = _meshFilter.mesh;

            string[] lines;

            lines = Parser.ParseTextFile(_nodesFile);

            Dictionary<SolidNodeFace, SolidNodeFace> faces = new Dictionary<SolidNodeFace, SolidNodeFace>();
            List<SolidEdge> edges;

            #region Create Vertices
            List<Vector3> tetrahedralVertices = new List<Vector3>();

            for (int i = 5; i < lines.Length; i += 4)
            {

                float x = float.Parse(lines[i], CultureInfo.InvariantCulture);
                float y = float.Parse(lines[i + 1], CultureInfo.InvariantCulture);
                float z = float.Parse(lines[i + 2], CultureInfo.InvariantCulture);
                tetrahedralVertices.Add(new Vector3(x, y, z));

            }

            CreateNodes(tetrahedralVertices.ToArray());

            #endregion

            #region Create Tetrahedrons. Wind Faces, Node masses and Edges for springs are set here too
            //Create Tetrahedral
            edges = new List<SolidEdge>();
            _faces = new List<SolidNodeFace>();

            lines = Parser.ParseTextFile(_eleFile);

            _weightedVertices = new WeightedVertex[_mesh.vertices.Length];
            bool[] setVertices = new bool[_mesh.vertices.Length];

            for (int i = 4; i < lines.Length; i += 5)
            {
                int aIndex;
                int bIndex;
                int cIndex;
                int dIndex;

                aIndex = int.Parse(lines[i], CultureInfo.InvariantCulture) - 1;
                bIndex = int.Parse(lines[i + 1], CultureInfo.InvariantCulture) - 1;
                cIndex = int.Parse(lines[i + 2], CultureInfo.InvariantCulture) - 1;
                dIndex = int.Parse(lines[i + 3], CultureInfo.InvariantCulture) - 1;

                Vector3 a = _nodes[aIndex].pos;
                Vector3 b = _nodes[bIndex].pos;
                Vector3 c = _nodes[cIndex].pos;
                Vector3 d = _nodes[dIndex].pos;

                float totalVolume = GetTetrahedralVolume(a, b, c, d);

                #region Edges
                edges.Add(new SolidEdge(aIndex, bIndex, totalVolume / 6));
                edges.Add(new SolidEdge(aIndex, cIndex, totalVolume / 6));
                edges.Add(new SolidEdge(aIndex, dIndex, totalVolume / 6));

                edges.Add(new SolidEdge(bIndex, cIndex, totalVolume / 6));
                edges.Add(new SolidEdge(bIndex, dIndex, totalVolume / 6));

                edges.Add(new SolidEdge(cIndex, dIndex, totalVolume / 6));
                #endregion

                #region Create Faces for Wind
                //Adding Faces abc, acd, abd, bcd
                var newNode = new SolidNodeFace(_nodes[aIndex], _nodes[bIndex], _nodes[cIndex]);
                if (faces.ContainsKey(newNode))
                {
                    faces.Remove(newNode);
                }
                else
                {
                    faces.Add(newNode, newNode);
                }

                newNode = new SolidNodeFace(_nodes[aIndex], _nodes[bIndex], _nodes[dIndex]);
                if (faces.ContainsKey(newNode))
                {
                    faces.Remove(newNode);
                }
                else
                {
                    faces.Add(newNode, newNode);
                }

                newNode = new SolidNodeFace(_nodes[aIndex], _nodes[cIndex], _nodes[dIndex]);
                if (faces.ContainsKey(newNode))
                {
                    faces.Remove(newNode);
                }
                else
                {
                    faces.Add(newNode, newNode);
                }

                newNode = new SolidNodeFace(_nodes[bIndex], _nodes[cIndex], _nodes[dIndex]);
                if (faces.ContainsKey(newNode))
                {
                    faces.Remove(newNode);
                }
                else
                {
                    faces.Add(newNode, newNode);
                }

                #endregion

                float nodeMass = _density * totalVolume * 0.25f;

                _nodes[aIndex].AddTetrahedronMass(nodeMass);
                _nodes[bIndex].AddTetrahedronMass(nodeMass);
                _nodes[cIndex].AddTetrahedronMass(nodeMass);
                _nodes[dIndex].AddTetrahedronMass(nodeMass);

                for (int j = 0; j < _mesh.vertices.Length; j++)
                {

                    if (setVertices[j])
                    {
                        continue;
                    }

                    //abc Side
                    Vector3 center = (a + b + c) / 3;
                    Vector3 vertexToCenter = center - transform.TransformPoint(_mesh.vertices[j]);

                    Vector3 normal = Vector3.Cross(c - a, b - a);
                    if (Vector3.Dot(vertexToCenter, normal) > 0)
                    {
                        //abd Side
                        center = (a + b + d) / 3;
                        vertexToCenter = center - transform.TransformPoint(_mesh.vertices[j]);

                        normal = Vector3.Cross(b - a, d - a);
                        if (Vector3.Dot(vertexToCenter, normal) > 0)
                        {
                            //acd Side
                            center = (a + c + d) / 3;
                            vertexToCenter = center - transform.TransformPoint(_mesh.vertices[j]);

                            normal = Vector3.Cross(d - a, c - a);

                            if (Vector3.Dot(vertexToCenter, normal) > 0)
                            {
                                //bcd Side
                                center = (b + c + d) / 3;
                                vertexToCenter = center - transform.TransformPoint(_mesh.vertices[j]);

                                normal = Vector3.Cross(c - b, d - c);
                                if (Vector3.Dot(vertexToCenter, normal) > 0)
                                {
                                    setVertices[j] = true;

                                    Vector3 p = transform.TransformPoint(_mesh.vertices[j]);

                                    float aWeight = GetTetrahedralVolume(p, b, c, d) / totalVolume;

                                    float bWeight = GetTetrahedralVolume(a, p, c, d) / totalVolume;

                                    float cWeight = GetTetrahedralVolume(a, b, p, d) / totalVolume;

                                    float dWeight = GetTetrahedralVolume(a, b, c, p) / totalVolume;

                                    _weightedVertices[j] = new WeightedVertex(_nodes[aIndex], aWeight, _nodes[bIndex], bWeight, _nodes[cIndex], cWeight, _nodes[dIndex], dWeight);

                                    //Debug.Log(aWeight + bWeight + cWeight + dWeight);
                                    //Debug.Log("Point " + p + " is inside of " + a + " " + b + " " + c + " " + d);

                                }


                            }
                        }


                    }

                }


            }

            foreach (SolidNode node in _nodes)
            {
                node.SetAverageMass();
            }

            foreach (SolidNodeFace f in faces.Values)
            {

                _faces.Add(f);
            }
            #endregion

            #region Create Springs
            Dictionary<SolidEdge, SolidSpring> unrepeatedSprings = new Dictionary<SolidEdge, SolidSpring>();
            SolidSpring spring;

            _springs = new List<SolidSpring>();
            foreach (SolidEdge newEdge in edges)
            {

                if (!unrepeatedSprings.TryGetValue(newEdge, out spring))
                {
                    spring = new SolidSpring(_nodes[newEdge.vertexA], _nodes[newEdge.vertexB]);
                    unrepeatedSprings.Add(newEdge, spring);

                    _springs.Add(spring);
                }

                spring.AddTetrahedronVolume(newEdge.volume);

            }


            foreach (SolidSpring s in _springs)
            {
                s.SetAverageVolume();
            }
            #endregion

            #region Enviroment
            Colliders = new List<Collider>();
            SolidFixer[] fixers = FindObjectsOfType<SolidFixer>();

            foreach (Collider collider in FindObjectsOfType<Collider>())
            {

                if (_collisionLayers == (_collisionLayers | (1 << collider.gameObject.layer)))
                {
                    Colliders.Add(collider);

                }
            }


            Planes = new List<Collider>();
            foreach (GameObject plane in GameObject.FindGameObjectsWithTag("Plane"))
            {

                if (_collisionLayers == (_collisionLayers | (1 << plane.gameObject.layer)))
                {
                    var collider = plane.GetComponent<MeshCollider>();
                    if (collider != null)
                        Planes.Add(collider);

                }
            }
            foreach (SolidFixer fixer in fixers)
            {
                foreach (SolidNode node in _nodes)
                {
                    if (fixer.PointIsInside(node.pos))
                    {
                        node.isFixed = true;

                        fixer.AddNode(node);
                    }
                }
            }

            _windColliders = FindObjectsOfType<WindCollider>();
            #endregion

        }
        else
        {
            Debug.LogWarning("No mesh in ElasticSolid " + name);
        }
    }

    public ElasticSolid()
    {
        _gravity = new Vector3(0, -9.81f, 0);
    }

    void CreateNodes(Vector3[] vertices)
    {
        _nodes = new List<SolidNode>();

        transform.localScale = transform.localScale * _nodeMeshScale;
        transform.localPosition = transform.localPosition + _nodeMeshOffset;
        transform.eulerAngles = transform.eulerAngles + _nodeMeshRotation;
        foreach (Vector3 vertex in vertices)
        {
            _nodes.Add(new SolidNode(transform.TransformPoint(vertex), this));
        }
        transform.localScale = transform.localScale / _nodeMeshScale;
        transform.localPosition = transform.localPosition - _nodeMeshOffset;
        transform.eulerAngles = transform.eulerAngles + _nodeMeshRotation;



    }

    void ComputeForces()
    {
        if (_nodes != null && _springs != null)
        {
            foreach (SolidNode node in _nodes)
            {
                node.force = Vector3.zero;
                node.ComputeForces(_nodeDamping, _gravity, _penaltyFactor);
            }

            foreach (SolidSpring spring in _springs)
            {
                spring.ComputeForces(_density, _springDamping, _springStiffness);
            }

            ComputeWindForce();
        }
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

    void StepExplicit()
    {
        if (_nodes != null)
        {
            foreach (SolidNode node in _nodes)
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
            foreach (SolidNode node in _nodes)
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

    private void UpdateVertices()
    {
        //Procedure to update vertex positions
        Vector3[] vertices = new Vector3[_mesh.vertexCount];

        for (int i = 0; i < vertices.Length; i++)
        {

            if (_weightedVertices[i].isSet)
            {
                vertices[i] = transform.InverseTransformPoint(_weightedVertices[i].GetPosition());
            }
        }

        _mesh.vertices = vertices;
    }
    void ComputeWindForce()
    {
        float windTimeFactor = Mathf.Abs(Mathf.Sin(windElapsedTime));

        windElapsedTime += _stepTime * _windChangeSpeed;

        foreach (SolidNodeFace face in _faces)
        {
            Vector3 windColliderVelocity = Vector3.zero;

            foreach (WindCollider collider in _windColliders)
            {
                if (collider.PointIsColliding(face.nodeA.pos)
                    || collider.PointIsColliding(face.nodeB.pos)
                    || collider.PointIsColliding(face.nodeC.pos))
                {
                    windColliderVelocity += collider.windVelocity;
                }
            }


            Vector3 normal = face.GetNormal();

            Vector3 triangleVelocity = face.nodeA.vel + face.nodeB.vel + face.nodeC.vel;
            triangleVelocity = triangleVelocity / 3;




            Vector3 windForce = windTimeFactor * face.GetArea() * (Vector3.Dot(normal, _windVelocity + windColliderVelocity - triangleVelocity)) * normal;

            windForce = windForce / 3;
            face.nodeA.force += windForce;
            face.nodeB.force += windForce;
            face.nodeC.force += windForce;


        }
    }

    float GetTetrahedralVolume(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        /*
        float volume = (b - a).magnitude * (c - a).magnitude * (d - a).magnitude;
        volume /= 6 * Mathf.Pow(2, 0.5f);
        */

        float volume = Mathf.Abs(Vector3.Dot(b - a, Vector3.Cross(c - a, d - a))) / 6;

        return volume;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_drawNodes)
        {
            if (_nodes != null)
            {
                for (int i = 0; i < _nodes.Count; i++)
                {
                    Gizmos.color = _nodeColor;
                    Gizmos.DrawSphere(_nodes[i].pos, _nodeRadius);

                    if (_drawNodeIndex)
                        Handles.Label(_nodes[i].pos + _indexOffset, "Node " + i);
                }
            }

        }

        if (_drawSprings && _springs != null)
        {
            foreach (SolidSpring spring in _springs)
            {
                Gizmos.color = _springColor;
                Gizmos.DrawLine(spring.nodeA.pos, spring.nodeB.pos);
            }
        }


    }
#endif


}
