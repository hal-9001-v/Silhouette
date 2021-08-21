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

    [Header("Rumbler")]
    [SerializeField] [Range(0, 1)] float _hitRumblerSpeed;
    [SerializeField] [Range(0, 1)] float _hitRumblerDuration;

    Melee _melee;

    Rumbler _rumbler;

    public Semaphore semaphore;

    private void Awake()
    {
        _melee = GetComponent<Melee>();
        _melee.hitAction += () =>
        {
            _rumbler.Rumble(_hitRumblerSpeed, _hitRumblerSpeed, _hitDuration);
        };

        _rumbler = FindObjectOfType<Rumbler>();

        semaphore = new Semaphore();
    }

    private void Start()
    {

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
