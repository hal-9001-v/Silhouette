using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SightTrigger))]
public class SpottedEvent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] UnityEvent _spottedEvent;

    // Start is called before the first frame update
    void Start()
    {
        var trigger = GetComponent<SightTrigger>();

        trigger.spottedAction += _spottedEvent.Invoke;
    }
}
