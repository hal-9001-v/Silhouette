using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CollisionInteractable : MonoBehaviour
{
    bool _enterDone;
    bool _exitDone;
    [SerializeField]bool _enterOnlyOnce;
    [SerializeField]bool _exitOnlyOnce;

    [SerializeField]UnityEvent _enterEvent;
    [SerializeField]UnityEvent _exitEvent;

    public void EnterInteraction()
    {
        if (_enterOnlyOnce && _enterDone)
            return;

        _enterDone = true;

        _enterEvent.Invoke();
       
    }

    public void ExitInteraction() {
        if (_exitOnlyOnce && _exitDone)
            return;

        _exitDone = true;

        _exitEvent.Invoke();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractionTrigger>() != null) {
            EnterInteraction();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<InteractionTrigger>() != null)
        {
            ExitInteraction();
        }
    }



}
