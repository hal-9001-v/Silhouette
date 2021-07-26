using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class WallZone : MonoBehaviour
{

    [Header("Settings")]
    public Vector3 Direction;
    public Transform[] points;

    public UnityEvent AtStartEvents;
    public UnityEvent AtEndEvents;

    [SerializeField] [Range(0.01f, 0.5f)] float _timeStep;

    [Header("Gizmos")]
    [SerializeField] [Range(0.1f, 1)] float _pointRadius;
    [SerializeField] [Range(0.1f, 1)] float _curvePointRadius;
    [SerializeField] [Range(0.1f, 1)] float _arrowRadius;
    [SerializeField] Vector3 _currentPositionCubeSize = new Vector3(1, 1, 1);
    [SerializeField] Color _forwardColor = Color.blue;
    [SerializeField] Color _curveColor = Color.green;
    [SerializeField] Color _pointColor = Color.red;
    [SerializeField] Color _currentPositionColor = Color.yellow;

    private float _currentTime;
    CharacterWallSneak _player;

    public float CurveLength { get; private set; }

    private void Awake()
    {
        CurveLength = GetCurveLength();
        _player = FindObjectOfType<CharacterWallSneak>();
    }


    Vector3 GetPositionInCurve(float time, Vector3 point0, Vector3 point1, Vector3 point2)
    {
        //Quadratic Bezier Curve
        //B(t) = (1-t) * P0 +              2t(1-t)*P1 +                   t^2*P2, te[0,1]
        return Mathf.Pow((1 - time), 2) * point0 + 2 * time * (1 - time) * point1 + (time * time) * point2;
    }

    Vector3 GetPositionInCurve(float time)
    {
        //Quadratic Bezier Curve
        return GetPositionInCurve(time, points[0].position, points[1].position, points[2].position);
    }

    Vector3 GetForwardDirection(float firstTime, float secondTime)
    {
        Vector3 firstPoint = GetPositionInCurve(firstTime, points[0].position, points[1].position, points[2].position);
        Vector3 secondPoint = GetPositionInCurve(secondTime, points[0].position, points[1].position, points[2].position);

        Vector3 right = firstPoint - secondPoint;
        right.Normalize();

        Vector3 forward = Vector3.Cross(Vector3.up, right);

        return forward;
    }

    public Vector3 GetCurrentForwardDirection()
    {

        Vector3 firstPoint = GetPositionInCurve(_currentTime * 0.99f, points[0].position, points[1].position, points[2].position);
        Vector3 secondPoint = GetPositionInCurve(_currentTime, points[0].position, points[1].position, points[2].position);

        Vector3 right = firstPoint - secondPoint;
        right.Normalize();

        Vector3 forward = Vector3.Cross(Vector3.up, right);

        return forward;

    }

    private void OnDrawGizmos()
    {
        if (points != null && points.Length >= 3)
        {

            for (int i = 0; i < points.Length; i++)
            {
                Gizmos.color = _pointColor;
                Gizmos.DrawSphere(points[i].position, _pointRadius);
            }

            for (int i = 0; i < Mathf.RoundToInt(1 / _timeStep); i++)
            {
                var fromPoint = GetPositionInCurve(i * _timeStep, points[0].position, points[1].position, points[2].position);
                var toPoint = GetPositionInCurve((i + 1) * _timeStep, points[0].position, points[1].position, points[2].position);

                Gizmos.DrawSphere(fromPoint, _curvePointRadius);

                Gizmos.color = _forwardColor;
                Gizmos.DrawLine(fromPoint, fromPoint + GetForwardDirection(i * _timeStep, (i + 1) * _timeStep));

                Gizmos.color = _curveColor;
                Gizmos.DrawLine(fromPoint, toPoint);
            }

            Gizmos.color = _currentPositionColor;
            Gizmos.DrawCube(GetPositionInCurve(_currentTime, points[0].position, points[1].position, points[2].position), _currentPositionCubeSize);
        }


    }

    public void ChangeTValue(Vector3 direction, float value)
    {
        if (points != null)
        {
            Vector3 centerToFirstPoint = points[0].position - transform.position;

            if (Vector3.Dot(centerToFirstPoint, direction) > 0)
            {
                _currentTime += value;
            }
            else {
                _currentTime -= value;
            } 

            if (_currentTime > 1)
            {
                _currentTime = 1;

            }
            else if (_currentTime < 0)
            {
                _currentTime = 0;
            }
        }

    }

    float GetCurveLength()
    {
        float length = 0;
        for (int i = 0; i < 9; i++)
        {

            var firstPoint = GetPositionInCurve(i * 0.1f, points[0].position, points[1].position, points[2].position);
            var secondPoint = GetPositionInCurve((i + 1) * 0.1f, points[0].position, points[1].position, points[2].position);
            length += Vector3.Distance(firstPoint, secondPoint);
        }

        return length;
    }

    public Vector3 GetCurrentPosition()
    {
        return GetPositionInCurve(_currentTime, points[0].position, points[1].position, points[2].position);
    }

    public void setCurrentPositionToClosestPoint(Vector3 point)
    {
        float time = 0;
        float closestTime = 0;

        while (time < 1)
        {
            var closestpoint = GetPositionInCurve(closestTime, points[0].position, points[1].position, points[2].position);
            var newPoint = GetPositionInCurve(time, points[0].position, points[1].position, points[2].position);

            if (Vector3.Distance(closestpoint, point) > Vector3.Distance(newPoint, point))
            {
                closestTime = time;
            }

            time += _timeStep;

        }

        _currentTime = closestTime;

    }

    public void StickPlayer()
    {

        _player.StickToWall(this);
        setCurrentPositionToClosestPoint(_player.transform.position);

        AtStartEvents.Invoke();
    }

    public void UnstickPlayer()
    {
        AtEndEvents.Invoke();

        _player.UnstickToWall();
    }

}
