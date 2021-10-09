using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GeneralTools;

[RequireComponent(typeof(NavMeshAgent), typeof(GroundChecker), typeof(Rigidbody))]
public class Navigator : MonoBehaviour
{
    const int JumpMeshLink = 2;


    [Header("Settings")]
    [SerializeField] [Range(0.01f, 1)] float _reachingDistance = 0.1f;
    [SerializeField] [Range(0.01f, 10)] float _warpDistance = 1f;

    public Action patrolPointReachedAction;
    public Action pursueReachedAction;
    Action _targetPositionReachedAction;

    PatrolList _patrolList;

    public Vector3 velocity
    {
        get
        {
            return _navMeshAgent.velocity;
        }
    }

    float _desiredSpeed;

    NavMeshAgent _navMeshAgent;
    Rigidbody _rigidbody;
    GroundChecker _groundChecker;

    Transform _target;
    //Used for an specific position with no changes
    Vector3 _targetPosition;

    NavState _currentState;

    enum NavState
    {
        Idle,

        Pursue,
        Patrol,
        GoingToPosition,

        Launch,
        Land
    }

    // Start is called before the first frame update
    void Awake()
    {
        _patrolList = new PatrolList();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();

        _rigidbody.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (_navMeshAgent.isOnNavMesh == false && _navMeshAgent.enabled)
        {
            WarpAgent();
        }

        switch (_currentState)
        {
            //PATROL STATE
            case NavState.Patrol:
                if (_patrolList.points.Length == 0) return;

                if (_navMeshAgent.enabled)
                    _navMeshAgent.SetDestination(_patrolList.GetCurrentPoint().position);

                if (HorizontalDistance(transform.position, _patrolList.GetCurrentPoint().position) < _reachingDistance)
                {
                    _patrolList.GetNextPoint();

                    if (patrolPointReachedAction != null)
                    {
                        patrolPointReachedAction.Invoke();
                    }
                }
                break;

            //PURSUE STATE
            case NavState.Pursue:
                if (_navMeshAgent.enabled)
                    _navMeshAgent.SetDestination(_target.position);

                if (HorizontalDistance(_navMeshAgent.transform.position, _target.position) < _reachingDistance)
                {
                    if (patrolPointReachedAction != null)
                        pursueReachedAction.Invoke();
                }

                if (_navMeshAgent.isOnOffMeshLink && _navMeshAgent.currentOffMeshLinkData.offMeshLink != null)
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

                if (_navMeshAgent.enabled)
                    _navMeshAgent.SetDestination(_targetPosition);

                if (HorizontalDistance(_navMeshAgent.transform.position, _targetPosition) < _reachingDistance)
                {
                    if (_targetPositionReachedAction != null)
                    {
                        _targetPositionReachedAction.Invoke();
                    }
                }

                break;

            case NavState.Launch:

                if (_groundChecker.isGrounded)
                {
                    ChangeState(NavState.Land);
                }

                break;

            default:
                break;
        }


    }

    public void DisableNavigator()
    {
        _rigidbody.isKinematic = false;

        _navMeshAgent.enabled = false;

        enabled = false;

    }

    public void EnableNavigator()
    {
        _rigidbody.isKinematic = true;

        _navMeshAgent.enabled = true;

        enabled = true;
    }

    public void SetPatrolRoute(PatrolRoute route)
    {
        _patrolList.SetPoints(route.patrolPoints);

        transform.position = _patrolList.GetCurrentPoint().position;
    }

    public void Patrol(float speed)
    {
        if (_patrolList.points != null)
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
    }

    public void Continue()
    {
        if (_navMeshAgent.isOnNavMesh)
            _navMeshAgent.isStopped = false;
    }

    public void Launch(Vector3 velocity)
    {
        ChangeState(NavState.Launch);

        _rigidbody.AddForce(velocity - _rigidbody.velocity, ForceMode.VelocityChange);

    }

    float HorizontalDistance(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
    }


    void ChangeState(NavState state)
    {
        if (state == _currentState) return;

        _currentState = state;

        switch (state)
        {
            case NavState.Idle:
                _navMeshAgent.velocity = Vector3.zero;

                Stop();
                break;

            case NavState.Patrol:
                _navMeshAgent.enabled = true;

                Continue();
                break;

            case NavState.Pursue:
                _navMeshAgent.enabled = true;

                Continue();
                break;

            case NavState.GoingToPosition:
                _navMeshAgent.enabled = true;

                Continue();
                break;

            case NavState.Launch:
                _navMeshAgent.enabled = false;
                _rigidbody.isKinematic = false;

                break;

            case NavState.Land:
                _navMeshAgent.enabled = true;
                _rigidbody.isKinematic = true;



                break;


        }



    }


    class PatrolList
    {
        public Transform[] points { get; private set; }
        int _index;

        public Transform GetNextPoint()
        {
            _index++;

            if (_index == points.Length) _index = 0;

            return points[_index];
        }

        public Transform GetCurrentPoint()
        {
            return points[_index];
        }

        public void SetPoints(Transform[] newPoints)
        {
            points = newPoints;
            _index = 0;

        }

    }

    public void WarpAgent()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, _warpDistance, NavMesh.AllAreas))
        {
            _navMeshAgent.Warp(hit.position);
        }
    }
}


