using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    public Action<float> waterContactAction;

    public void WaterContact(float damage)
    {
        if (waterContactAction != null)
            waterContactAction.Invoke(damage);
    }

}
