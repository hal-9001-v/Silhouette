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

    private void Awake()
    {
        _melee = GetComponent<Melee>();
    }

    public override void SetInput(PlatformMap input)
    {
        input.Character.Attack.performed += ctx =>
        {
            _melee.Attack(_damage, _push, _hitDuration);
        };
    }
}
