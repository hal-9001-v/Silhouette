using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneRoute : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform _destination;

    [Header("Settings")]
    [SerializeField] [Range(1, 15)] float _speed;

    [SerializeField] UnityEvent _atArrivingEvent;

    CharacterNavigator _characterNavigator;

    private void Awake()
    {
        _characterNavigator = FindObjectOfType<CharacterNavigator>();
    }

    public void TriggerRoute()
    {
        if (_destination)
        {
            if (_characterNavigator)
            {
                _characterNavigator.GoToDestination(_destination.position, _speed, _atArrivingEvent.Invoke);
            }
        }
    }

}
