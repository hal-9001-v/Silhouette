using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Patroller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _body;
    [SerializeField] Transform[] _patrolPoints;

    [Header("Movement")]
    [SerializeField] [Range(0.1f, 5)] float _rotationLerpFactor = 0.5f;
    [SerializeField] [Range(0.1f, 10)] float _patrolSpeed = 3;

    //Patrol Variables
    int _currentPatrolPoint = 0;
    float _elapsedTime = 0;
    float _movementLerpDuration = 1;
    Vector3 _lerpStartPosition;
    Vector3 _previousPosition;

    private void Start()
    {

        if (_body == null)
        {
            Debug.LogWarning("No body attribute in " + name + "'s Patroller component!");
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Length != 0)
            {

                //Set all points on the same height and set point's parents null On Game mode
                for (int i = 0; i < _patrolPoints.Length; i++)
                {
                    if (_patrolPoints[i] != null)
                    {
                        var newPos = _patrolPoints[i].position;
                        newPos.y = _body.transform.position.y;

                        _patrolPoints[i].position = newPos;

                        _patrolPoints[i].parent = null;
                    }
                }

                if (_patrolPoints[0] != null)
                {
                    _body.transform.position = _patrolPoints[0].position;
                    _lerpStartPosition = _patrolPoints[0].position;

                    if (_patrolPoints[1] != null)
                    {
                        _body.transform.LookAt(_patrolPoints[1]);
                    }

                }
            }

        }

    }

    /// <summary>
    /// Rotate and move patroller in a frame.
    /// </summary>
    public void ApplyPatrol()
    {
        ApplyMovement();
        RotateTowardsMovement();
    }

    /// <summary>
    /// Move patroller in a frame
    /// </summary>
    public void ApplyMovement()
    {
        if (_body != null && _patrolPoints != null && _patrolPoints.Length != 0)
        {
            _body.transform.position = Vector3.Lerp(_lerpStartPosition, _patrolPoints[_currentPatrolPoint].position, (_elapsedTime / _movementLerpDuration));

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _movementLerpDuration)
            {
                SetNextPatrolPoint();
            }

        }

    }

    /// <summary>
    /// Rotate patroller in a frame.
    /// </summary>
    public void RotateTowardsMovement()
    {
        if (_body != null)
        {
            Vector3 direction = _body.transform.position - _previousPosition;
            _previousPosition = _body.transform.position;

            if (direction != Vector3.zero)
            {
                direction.Normalize();

                Quaternion prevRotation = _body.transform.rotation;
                Quaternion actualRot = Quaternion.LookRotation(direction);

                var rot = Quaternion.Lerp(prevRotation, actualRot, Time.deltaTime * _rotationLerpFactor).eulerAngles;

                rot.z = 0;
                rot.x = 0;
                _body.transform.eulerAngles = rot;
            }
        }


    }

    void SetNextPatrolPoint()
    {

        if (_patrolPoints != null)
        {
            _currentPatrolPoint++;
            if (_currentPatrolPoint >= _patrolPoints.Length)
            {
                _currentPatrolPoint = 0;

            }

            _movementLerpDuration = Vector3.Distance(_body.transform.position, _patrolPoints[_currentPatrolPoint].position) / _patrolSpeed;
            _lerpStartPosition = _body.transform.position;
            _elapsedTime = 0;
        }

    }

    [ContextMenu("Create Patrol Point")]
    void CreatePatrolPoint()
    {
        GameObject go = new GameObject();
        go.name = name + "'s Patrol Point";
        go.transform.position = transform.position;

        go.transform.parent = transform;
    }

    private void OnDrawGizmos()
    {
        if (_patrolPoints != null)
        {
            for (int i = 0; i < _patrolPoints.Length; i++)
            {
                if (_patrolPoints[i] != null)
                {

                    if (i == 0)
                        Gizmos.color = Color.blue;
                    else
                        Gizmos.color = Color.green;

                    Gizmos.DrawCube(_patrolPoints[i].position, new Vector3(1f, 1f, 1f));

                    if (i + 1 < _patrolPoints.Length && _patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(_patrolPoints[i].position, _patrolPoints[i + 1].position);
                    }
                }

            }


        }

    }
}
