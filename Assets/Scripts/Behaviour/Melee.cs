using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MeleeAttack[] _meleeAttacks;
    [SerializeField] Animator _animator;

    public Action hitAction;
    public Action endOfAttackAction;

    bool _attacking;

    const string AttackTrigger = "Attack";
    const string AttackInteger = "AttackIndex";

    private void Start()
    {
        if (_meleeAttacks != null)
        {
            foreach (var meleeAttack in _meleeAttacks)
            {
                meleeAttack.Initialize(hitAction);
            }
        }
    }
    public void Attack(int index)
    {
        if (_attacking == false)
        {
            if (_meleeAttacks != null && _meleeAttacks.Length != 0)
            {
                if (index >= 0 && index < _meleeAttacks.Length)
                {
                    _attacking = true;
                    StartCoroutine(AttackMovement(index));
                }
            }
        }
    }

    IEnumerator AttackMovement(int index)
    {
        MeleeAttack attack = _meleeAttacks[index];
        _animator.SetInteger(AttackInteger, index);
        _animator.SetTrigger(AttackTrigger);

        attack.EnableColliders();
        yield return new WaitForSeconds(attack.hitDuration);
        attack.DisableColliders();

        _attacking = false;

        if (endOfAttackAction != null)
            endOfAttackAction.Invoke();
    }
}

[Serializable]
public class MeleeAttack
{
    public Collider[] hitColliders;
    public float hitDuration;
    public float damage;
    public float push;

    private Action _hitAction;

    public void Initialize(Action hitAction)
    {
        if (hitColliders != null)
        {
            _hitAction = hitAction;

            foreach (Collider collider in hitColliders)
            {
                var colliderDelegate = collider.gameObject.AddComponent<ColliderDelegate>();

                colliderDelegate.TriggerEnterAction += (source, coll, pos) =>
                {
                    Hit(source, coll, pos);
                };

                collider.enabled = false;
            }
        }
    }

    public void EnableColliders()
    {
        foreach (Collider collider in hitColliders)
        {
            collider.enabled = true;
        }

    }

    public void DisableColliders()
    {
        foreach (Collider collider in hitColliders)
        {
            collider.enabled = false;
        }

    }

    private void Hit(Transform source, Collider coll, Vector3 pos)
    {
        var health = coll.GetComponent<Health>();
        if (coll.isTrigger == false && health)
        {
            //Debug.Log("HURT");
            health.Hurt(damage, pos, push, source);

            if (_hitAction != null)
                _hitAction.Invoke();

        }
    }

}