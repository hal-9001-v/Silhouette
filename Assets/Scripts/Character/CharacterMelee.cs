using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Melee))]
public class CharacterMelee : InputComponent
{

    [Header("Settings")]
    [SerializeField] [Range(0, 10)] float _damage;
    [SerializeField] [Range(0, 20)] float _push;
    [SerializeField] [Range(0, 20)] float _hitDuration;
    Melee _melee;

    int _lockCount;

    private void Awake()
    {
        _melee = GetComponent<Melee>();
    }

    public void Lock()
    {
        _lockCount++;
    }

    public void Unlock()
    {

        if (_lockCount > 0) _lockCount--;
        else
        {
            Debug.LogWarning("Lock Count is already 0, cant be freed!");
        }
    }

    public override void SetInput(PlatformMap input)
    {
        input.Character.Attack.performed += ctx =>
        {
            if (_lockCount == 0)
                _melee.Attack(_damage, _push, _hitDuration);
        };
    }
}
