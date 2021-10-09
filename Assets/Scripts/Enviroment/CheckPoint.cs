using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isDefaultCheckPoint;

    public void SetCheckPoint()
    {

        var character = FindObjectOfType<CharacterHealth>();

        if (character)
        {
            character.activeCheckPoint = this;
        }
    }

    public static CheckPoint GetDefaultCheckPoint()
    {
        var checkPoints = FindObjectsOfType<CheckPoint>();

        if (checkPoints != null)
        {
            foreach (var checkPoint in checkPoints)
            {
                if (checkPoint.isDefaultCheckPoint)
                {
                    return checkPoint;
                }
            }
        }

        return null;
    }

}
