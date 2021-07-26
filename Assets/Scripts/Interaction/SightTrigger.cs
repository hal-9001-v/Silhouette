using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightTrigger : MonoBehaviour
{
    [Header("Settings")]

    //Distance To Center
    [SerializeField] [Range(0.1f, 2f)] float _xRange;
    //Segments/2 for X axis
    [SerializeField] [Range(0, 10)] int _xSegments = 2;

    [Space(6)]
    [SerializeField] [Range(0.1f, 2f)] float _yRange;
    //Segments/2 for Y axis
    [SerializeField] [Range(0, 10)] int _ySegments = 2;

    [Space(6)]
    [SerializeField] [Range(0.1f, 2f)] float _zRange;
    //Segments/2 for Z axis
    [SerializeField] [Range(0, 10)] int _zSegments = 2;

    [Space(6)]
    //Offset to match Model's center
    [SerializeField] Vector3 _modelOffset;


    [Header("Gizmos")]
    [SerializeField] bool _drawGizmos = true;
    [SerializeField] [Range(0, 1)] float _pointRadius = 0.2f;
    [SerializeField] Color _pointColor = Color.blue;

    //Variables
    public Vector3 SightCenter
    {
        get
        {
            return transform.position + _modelOffset;
        }
        private set
        {

        }
    }

    public Vector3[] GetSightPoints()
    {
        Gizmos.color = _pointColor;
        //Each axis gets x2 and there are 3
        Vector3[] points = new Vector3[_xSegments * 2 + _ySegments * 2 + _zSegments * 2 + 1];

        points[0] = transform.position + _modelOffset;

        int pointCounter = 1;

        //X Points
        Vector3 segmentOffset = _xRange * transform.right / _xSegments;
        Vector3 centerOffset = transform.position + _modelOffset + segmentOffset * 0.5f;
        for (int i = -_xSegments; i < _xSegments; i++)
        {
            points[pointCounter] = centerOffset + segmentOffset * i;

            pointCounter++;
        }

        //Y Points
        segmentOffset = _yRange * transform.up / _ySegments;
        centerOffset = transform.position + _modelOffset + segmentOffset * 0.5f;
        for (int i = -_ySegments; i < _ySegments; i++)
        {
            points[pointCounter] = centerOffset + segmentOffset * i;

            pointCounter++;
        }

        //Z Points        
        segmentOffset = _zRange * transform.forward / _zSegments;
        centerOffset = transform.position + _modelOffset + segmentOffset * 0.5f;
        for (int i = -_zSegments; i < _zSegments; i++)
        {
            points[pointCounter] = centerOffset + segmentOffset * i;

            pointCounter++;
        }

        return points;
    }


    private void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            Vector3[] points = GetSightPoints();

            foreach (Vector3 p in points)
            {
                Gizmos.DrawSphere(p, _pointRadius);
            }
        }


    }

}
