using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class HurtEvent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] UnityEvent _hurtEvent;
    [SerializeField] UnityEvent _deadEvent;

    private void Start()
    {
        var health = GetComponent<Health>();

        health.hurtAction += InvokeHurtEvent;
        health.deadAction += InvokeDeadEvent;

    }
    void InvokeHurtEvent(Vector3 vector, float push, Transform source)
    {
        _hurtEvent.Invoke();
    }
    void InvokeDeadEvent(Vector3 vector, float push, Transform source)
    {
        _deadEvent.Invoke();
    }

}
