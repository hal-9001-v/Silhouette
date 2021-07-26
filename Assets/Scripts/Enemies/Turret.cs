using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeReference] Transform _head;
    [SerializeReference] Transform _spawn;
    [SerializeReference] GameObject _projectilePrefab;
    [SerializeReference] Animator _animator;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 2)] float _undeployTime;

    Shooter _shooter;
    Transform _target;


    Coroutine _undeployCoroutine;
    bool _isShooting;

    //Animator
    const string DeployedBool = "isDeployed";

    private void Awake()
    {
        _shooter = GetComponent<Shooter>();

        #region Shooter settings
        _shooter.StartAction += () =>
        {
            if (_undeployCoroutine != null)
            {
                StopCoroutine(_undeployCoroutine);
            }

            _isShooting = true;
        };

        _shooter.StopAction += () =>
        {
            if (_animator != null)
                _undeployCoroutine = StartCoroutine(Undeploy(_undeployTime));

            _isShooting = false;
        };
        #endregion

        _target = FindObjectOfType<SightTrigger>().transform;


    }

    private void FixedUpdate()
    {
        if (_target != null && _head != null)
        {
            _head.transform.LookAt(_target.position);
            Vector3 forward = _target.position - _head.position;

            forward.y = 0;

            _head.forward = forward.normalized;
        }


    }

    public void ShootTarget()
    {
        if (!_isShooting)
        {
            Debug.Log("There");
            if (_animator != null)
            {
                _animator.SetBool(DeployedBool, true);
            }

            _shooter.ShootToTarget(_target);
        }
    }

    IEnumerator Undeploy(float time)
    {
        yield return new WaitForSeconds(time);

        if (_animator != null)
        {
            _animator.SetBool(DeployedBool, false);
        }
    }

}
