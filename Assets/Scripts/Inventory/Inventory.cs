using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int points { get; private set; }
    public const int MaxPoints = 9999;

    [SerializeField] List<Item> _items;

    public Action changeInPointsAction;

    private void Awake()
    {
        _items = new List<Item>();
    }

    public void AddPoints(int addPoints)
    {
        points += Mathf.Abs(addPoints);


        if (points > MaxPoints)
        {
            points = MaxPoints;
        }

        if (changeInPointsAction != null)
        {
            changeInPointsAction.Invoke();
        }


    }

    public void RemovePoints(int removePoints)
    {
        points -= Mathf.Abs(removePoints);

        if (points < 0)
        {
            points = 0;
        }

        if (changeInPointsAction != null)
        {
            changeInPointsAction.Invoke();
        }

    }

    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public int ContainsItem(Item item)
    {
        int counter = 0;
        foreach (Item listItem in _items)
        {
            if (item.itemName.Equals(listItem.itemName))
                counter++;
        }

        return counter;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (item.itemName.Equals(_items[i].itemName))
            {
                _items.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void ClearItems()
    {
        _items.Clear();
    }

}
