using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Navigator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] PatrolRoute _patrolRoute;
    [SerializeField] [Range(0.01f, 1)] float _patrolDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _pursueDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _checkingPlaceDistance = 0.1f;

    public Action patrolPointReachedAction;
    public Action pursueReachedAction;
    public Action targetPositionReachedAction;

    int _currentPatrolPoint;
    bool _apply;

    Transform _target;
    EnviromentInfo _targetEnviromentInfo;

    //Used for an specific position with no changes
    Vector3 _targetPosition;

    State _currentState;

    enum State
    {
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

    void SetNextPatrolPoint()
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
        }
    }

    public void SetPatrolRoute(PatrolRoute route)
    {
        _patrolRoute = route;
        _currentPatrolPoint = 0;

        transform.position = route.patrolPoints[_currentPatrolPoint].position;
    }

    public void Patrol(float speed)
    {
        if (_patrolRoute != null)
        {
            _navMeshAgent.speed = speed;

            _target = _patrolRoute.patrolPoints[_currentPatrolPoint];

            _currentState = State.Patrol;

            Continue();
        }
    }

    public void Pursue(float speed, Transform target)
    {
        _navMeshAgent.speed = speed;

        _target = target;
        _targetEnviromentInfo = _target.GetComponent<EnviromentInfo>();

        _currentState = State.Pursue;

        Continue();
    }

    public void GoToPosition(float speed, Vector3 position)
    {
        _navMeshAgent.speed = speed;

        _targetPosition = position;

        _currentState = State.GoingToPosition;

        Continue();

    }

    public void Stop()
    {
        _navMeshAgent.isStopped = true;
        _apply = false;
    }

    public void Continue()
    {
        _navMeshAgent.isStopped = false;
        _apply = true;
    }

    private void FixedUpdate()
    {
        if (_apply)
        {
            if (_target != null)
            {
                //Update Target's position. SetDestination works with Vector3
                _navMeshAgent.SetDestination(_target.position);
            }
            else
            {
                //If there is no  target, there is no point on Fixed Update since it moves character towards target
                return;
            }

            switch (_currentState)
            {
                //PATROL STATE
                case State.Patrol:
                    if (_patrolRoute == null) return;
                    if (Vector3.Distance(_navMeshAgent.transform.position, _target.position) < _patrolDistance)
                    {
                        SetNextPatrolPoint();
                        _target = _patrolRoute.patrolPoints[_currentPatrolPoint];

                        if (patrolPointReachedAction != null)
                        {
                            patrolPointReachedAction.Invoke();
                        }
                    }
                    break;

                //PURSUE STATE
                case State.Pursue:
                    if (Vector3.Distance(_navMeshAgent.transform.position, _target.position) < _pursueDistance)
                    {
                        if (patrolPointReachedAction != null)
                            pursueReachedAction.Invoke();
                    }
                    break;

                case State.GoingToPosition:

                    _navMeshAgent.SetDestination(_targetPosition);

                    if (Vector3.Distance(_navMeshAgent.transform.position, _targetPosition) < _checkingPlaceDistance)
                    {
                        if (targetPositionReachedAction != null)
                        {
                            targetPositionReachedAction.Invoke();
                        }
                    }
                    break;

                default:
                    break;
            }


        }
    }

}
