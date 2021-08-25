using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class CharacterInventory : MonoBehaviour
{
    UICommand _command;
    Inventory _inventory;


    private void Start()
    {
        _inventory = GetComponent<Inventory>();
        _command = FindObjectOfType<UICommand>();

        _inventory.changeInPointsAction += UpdateMoney;

        UpdateMoney();
    }

    void UpdateMoney()
    {
        if (_command != null)
        {
            _command.SetMoney(_inventory.points);
        }
    }

}
