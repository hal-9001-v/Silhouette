using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mob))]
public class CombatEvent : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] UnityEvent _startFightAction;

    private void Start()
    {
        var mob = GetComponent<Mob>();


        mob.startFightAction += _startFightAction.Invoke;
    }

}
