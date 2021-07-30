using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMelee : InputComponent
{
    [Header("References")]
    [SerializeField] Collider[] _hitColliders;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 1)] float _hitDuration;

    bool _attacking;

    private void Awake()
    {
        foreach (Collider collider in _hitColliders)
        {
            var colliderDelegate = collider.gameObject.AddComponent<ColliderDelegate>();

            colliderDelegate.TriggerEnterAction += coll =>
            {
                Hit(coll);

            };
        }
    }

    private void Hit(Collider coll)
    {

    }

    private void Attack()
    {
        if (_attacking == false)
        {
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
    }

    public override void SetInput(PlatformMap input)
    {
        input.Character.Attack.performed += ctx =>
        {
            Attack();
        };
    }
}
