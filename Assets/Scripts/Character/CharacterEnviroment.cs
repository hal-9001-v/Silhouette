using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEnviroment : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 10)] float _closeDistance;
    [SerializeField] CharacterBodyRotation _characterBodyRotation;
    [SerializeField] CharacterMovement _characterMovement;
    [SerializeField] Rigidbody _rigidbody;
  
    MobRegister mobRegister;

    public bool isEnemyClose { get; private set; }
    public bool isDiscovered
    {
        get
        {
            return discoverCount > 0;
        }
    }

    public int discoverCount;

    public HeavyMob closestMob { get; private set; }

    private void Awake()
    {
       mobRegister = FindObjectOfType<MobRegister>();

    }

    private void FixedUpdate()
    {
        float distance;
        closestMob = ClosestMob(out distance);

        if (distance < _closeDistance)
        {
            isEnemyClose = true;
        }
        else
        {
            isEnemyClose = false;

        }



        //Decide Rotation
        if (isEnemyClose && !_characterMovement.isSprinting)
        {

            _characterBodyRotation.SetTargetRotation(closestMob.transform);
        }
        else
        {

            _characterBodyRotation.SetMovementRotation(_rigidbody);
        }

        //Gather enemies around
        if (isDiscovered) {
            WarnMobs();
        }

    }

    void WarnMobs() {
        foreach (var mob in mobRegister.mobs)
        {
            


        }
    }

    HeavyMob ClosestMob(out float distance)
    {
        distance = float.MaxValue;
        HeavyMob closestMob = null;

        foreach (HeavyMob mob in mobRegister.mobs)
        {
            var newDistance = Vector3.Distance(transform.position, mob.transform.position);
            if (newDistance < distance)
            {
                closestMob = mob;
                distance = newDistance;
            }
        }



        return closestMob;
    }


}
