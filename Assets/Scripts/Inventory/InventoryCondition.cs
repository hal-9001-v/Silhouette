using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryCondition : MonoBehaviour
{
    [SerializeField] ItemCondition[] _itemConditions;

    [SerializeField] UnityEvent _successEvent;
    [SerializeField] UnityEvent _failEvent;

    public void CheckCondition()
    {
        if (CheckCondition(FindObjectOfType<Inventory>()))
        {
            _successEvent.Invoke();
        }
        else
        {
            _failEvent.Invoke();
        }
    }

    public bool CheckCondition(Inventory inventory)
    {
        if (inventory == null) return false;

        foreach (ItemCondition condition in _itemConditions)
        {
            if (inventory.ContainsItem(condition.item) != condition.number)
            {
                return false;
            }

        }
        return true;

    }

    [Serializable]
    class ItemCondition
    {
        public Item item;
        public int number;
    }
}
