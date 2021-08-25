using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
    [Header("References")]
    [Tooltip("This GameObject will be destroyed if DestroyThis() is called")]
    [SerializeField] GameObject _parentForDestruction;

    [Header("Settings")]
    [SerializeField] UnityEvent _collectedUnityEvent;

    public Action<Inventory> collectedAction;

    private void OnTriggerEnter(Collider other)
    {

        var inventory = other.GetComponent<Inventory>();

        if (inventory)
        {
            _collectedUnityEvent.Invoke();

            if (collectedAction != null)
            {
                collectedAction.Invoke(inventory);
            }
        }
    }

    public void DestroyThis()
    {
        if (_parentForDestruction != null)
            Destroy(_parentForDestruction);
        else
            Destroy(gameObject);
    }

}
