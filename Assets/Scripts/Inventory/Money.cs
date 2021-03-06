using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collectable), typeof(Rigidbody))]
public class Money : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] [Range(0, 10)] int _value;



    // Start is called before the first frame update
    void Start()
    {
        var collectable = GetComponent<Collectable>();

        collectable.collectedAction += AddMoney;

    }

    public void AddMoney(Inventory inventory)
    {
        inventory.AddPoints(_value);
    }



}
