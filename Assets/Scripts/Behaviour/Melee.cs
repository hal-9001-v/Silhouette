using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider[] _hitColliders;

    [Header("Settings")]
    float _hitDuration;

    public Action hitAction;
    public Action endOfAttackAction;

    float _damage;
    float _push;

    bool _attacking;

    private void Awake()
    {
        if (_hitColliders != null)
        {

            foreach (Collider collider in _hitColliders)
            {
                var colliderDelegate = collider.gameObject.AddComponent<ColliderDelegate>();

                colliderDelegate.TriggerEnterAction += (coll, pos) =>
                {
                    Hit(coll, pos);

                };

                collider.enabled = false;
            }
        }
    }

    private void Hit(Collider coll, Vector3 pos)
    {
        var health = coll.GetComponent<Health>();
        if (health)
        {
            //Debug.Log("HURT");
            health.Hurt(_damage, pos, _push, transform);

            if (hitAction != null)
                hitAction.Invoke();

        }
    }

    public void Attack(float damage, float push, float duration)
    {
        if (_attacking == false)
        {
            _damage = damage;
            _push = push;
            _hitDuration = duration;

            _attacking = true;

            StartCoroutine(AttackMovement());
        }
    }

    IEnumerator AttackMovement()
    {

        foreach (Collider collider in _hitColliders)
        {
            collider.enabled = true;
        }

        yield return new WaitForSeconds(_hitDuration);

        foreach (Collider collider in _hitColliders)
        {
            collider.enabled = false;
        }

        _attacking = false;

        if (endOfAttackAction != null)
            endOfAttackAction.Invoke();
    }
}
