using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneCollider : MonoBehaviour
{
    InteractionTrigger _player;

    private void Awake()
    {
        _player = FindObjectOfType<InteractionTrigger>();
    }

    public Action EnterActions;

    public Action ExitActions;

    private void OnTriggerEnter(Collider other)
    {
        if (_player != null) {
            if (other.gameObject == _player.gameObject) {
                EnterActions.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_player != null) {
            if (other.gameObject == _player.gameObject)
            {
                ExitActions.Invoke();
            }
        }
    }

}
