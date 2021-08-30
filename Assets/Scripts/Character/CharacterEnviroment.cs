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
    [SerializeField] CharacterVent _characterVent;

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

    public Mob closestMob { get; private set; }

    private void Awake()
    {
        mobRegister = FindObjectOfType<MobRegister>();

    }

    private void Update()
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

        if (_characterVent.isOnVent == false)
        {
            //Decide Rotation
            if (isEnemyClose && !_characterMovement.isSprinting)
            {

                _characterBodyRotation.SetTargetRotation(closestMob.transform);
            }
            else
            {

                _characterBodyRotation.SetMovementRotation();
            }

        }
        //Gather enemies around
        if (isDiscovered)
        {
            WarnMobs();
        }

    }

    void WarnMobs()
    {
        foreach (var mob in mobRegister.mobs)
        {



        }
    }

    Mob ClosestMob(out float distance)
    {
        distance = float.MaxValue;
        Mob closestMob = null;

        foreach (Mob mob in mobRegister.mobs)
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
