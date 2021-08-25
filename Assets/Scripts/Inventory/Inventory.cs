using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int points { get; private set; }
    public const int MaxPoints = 9999;

    public Action changeInPointsAction;

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

}
