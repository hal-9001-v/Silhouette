using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GeneralTools;

[RequireComponent(typeof(NavMeshAgent), typeof(GroundChecker))]
public class Navigator : MonoBehaviour
{
    const int JumpMeshLink = 2;
    
    [Header("References")]
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] PatrolRoute _patrolRoute;

    [Header("Settings")]
    [SerializeField] [Range(0.01f, 1)] float _patrolDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _pursueDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _checkingPlaceDistance = 0.1f;

    public Action patrolPointReachedAction;
    public Action pursueReachedAction;
    Action _targetPositionReachedAction;

    public Vector3 velocity
    {
        get
        {
            return _navMeshAgent.velocity;
        }
    }

    float _desiredSpeed;

    int _currentPatrolPoint;

    Transform _target;

    //Used for an specific position with no changes
    Vector3 _targetPosition;

    NavState _currentState;

    enum NavState
    {
        Idle,

        Pursue,
        Patrol,
        GoingToPosition
    }

    // Start is called before the first frame update
    void Awake()
    {
        _currentPatrolPoint = -1;
        SetNextPatrolPoint();
    }

    Transform SetNextPatrolPoint()
    {
        if (_patrolRoute != null && _patrolRoute.patrolPoints.Length != 0)
        {
            if (_currentPatrolPoint >= _patrolRoute.patrolPoints.Length - 1)
            {
                _currentPatrolPoint = 0;
            }
            else
            {
                _currentPatrolPoint++;
            }

            return _patrolRoute.patrolPoints[_currentPatrolPoint];
        }

        return null;
    }

    public void SetPatrolRoute(PatrolRoute route)
    {
        _patrolRoute = route;
        _currentPatrolPoint = 0;

        transform.position = route.patrolPoints[_currentPatrolPoint].position;
    }

    public void WarpNavMesh()
    {
        _navMeshAgent.Warp(transform.position);
    }

    public void Patrol(float speed)
    {
        if (_patrolRoute != null)
        {
            _desiredSpeed = speed;
            _navMeshAgent.speed = speed;

            ChangeState(NavState.Patrol);

            Continue();
        }
        else
        {
            ChangeState(NavState.Idle);
        }
    }

    public void Pursue(float speed, Transform target)
    {
        _desiredSpeed = speed;
        _navMeshAgent.speed = speed;

        _target = target;

        ChangeState(NavState.Pursue);

        Continue();
    }

    public void GoToPosition(float speed, Vector3 position, Action arrivedAction)
    {
        _navMeshAgent.speed = speed;

        _targetPosition = position;

        _targetPositionReachedAction = arrivedAction;

        ChangeState(NavState.GoingToPosition);

        Continue();

    }

    public void Stop()
    {
        if (_navMeshAgent.isOnNavMesh)
            _navMeshAgent.isStopped = true;

        enabled = false;

        ChangeState(NavState.Idle);
    }

    public void Continue()
    {
        if (_navMeshAgent.isOnNavMesh)
            _navMeshAgent.isStopped = false;

        enabled = true;
    }

    float HorizontalDistance(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
    }

    private void FixedUpdate()
    {
        if (!_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.Warp(transform.position);
        }

        switch (_currentState)
        {
            //PATROL STATE
            case NavState.Patrol:
                if (_patrolRoute == null) return;

                if (HorizontalDistance(_navMeshAgent.transform.position, _targetPosition) < _patrolDistance)
                {
                    _targetPosition = SetNextPatrolPoint().position;
                    _navMeshAgent.SetDestination(_targetPosition);

                    if (patrolPointReachedAction != null)
                    {
                        patrolPointReachedAction.Invoke();
                    }
                }
                break;

            //PURSUE STATE
            case NavState.Pursue:
                _navMeshAgent.SetDestination(_target.position);

                if (HorizontalDistance(_navMeshAgent.transform.position, _target.position) < _pursueDistance)
                {
                    if (patrolPointReachedAction != null)
                        pursueReachedAction.Invoke();
                }

                if (_navMeshAgent.isOnOffMeshLink)
                {
                    if (_navMeshAgent.currentOffMeshLinkData.offMeshLink.area == JumpMeshLink)
                    {
                        _navMeshAgent.speed = _desiredSpeed * 2;
                    }
                }
                else
                {
                    _navMeshAgent.speed = _desiredSpeed;
                }

                break;



            case NavState.GoingToPosition:

                _navMeshAgent.SetDestination(_targetPosition);

                if (HorizontalDistance(_navMeshAgent.transform.position, _targetPosition) < _checkingPlaceDistance)
                {
                    if (_targetPositionReachedAction != null)
                    {
                        _targetPositionReachedAction.Invoke();
                    }
                    ChangeState(NavState.Idle);
                }

                break;

            default:
                break;
        }


    }

    void ChangeState(NavState state)
    {
        if (state == _currentState) return;

        _currentState = state;

        switch (state)
        {
            case NavState.Idle:
                _navMeshAgent.velocity = Vector3.zero;
                _navMeshAgent.enabled = false;
                break;

            case NavState.Patrol:
                _navMeshAgent.enabled = true;

                _targetPosition = _patrolRoute.patrolPoints[_currentPatrolPoint].position;
                
                Continue();
                break;

            case NavState.Pursue:
                _navMeshAgent.enabled = true;

                Continue();
                break;

            case NavState.GoingToPosition:
                _navMeshAgent.enabled = true;

                break;


        }



    }

}
