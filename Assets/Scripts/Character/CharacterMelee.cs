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

    public Semaphore semaphore;

    private void Awake()
    {
        _melee = GetComponent<Melee>();

        semaphore = new Semaphore();
    }




    public override void SetInput(PlatformMap input)
    {
        input.Character.Attack.performed += ctx =>
        {
            if (semaphore.isOpen)
                _melee.Attack(0);
        };
    }
}
