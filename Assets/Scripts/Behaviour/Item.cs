using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collectable))]
public class Item : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] string _name;

    private void Awake()
    {
        var collectable = GetComponent<Collectable>();

        collectable.collectedAction += AddItem;
    }

    void AddItem(Inventory inventory)
    {
        inventory.AddItem(this);
    }

    public string itemName
    {
        get
        {
            return _name;
        }
    }
}
