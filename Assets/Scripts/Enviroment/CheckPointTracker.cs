using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTracker : MonoBehaviour
{
    public CheckPoint currentCheckPoint { get; private set; }

    public void SetCheckPoint(CheckPoint checkPoint) {
        if (currentCheckPoint != null) {
            currentCheckPoint.DisableCheckPoint(this);
        }

        currentCheckPoint = checkPoint;

    }


    public void SpawnAtCheckPoint() {
        if (currentCheckPoint != null) {
            transform.position = currentCheckPoint.SpawnPosition;
        }
    
    }

}
