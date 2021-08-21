using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    public Action<float, WaterBody> waterContactAction;

    public void WaterContact(float damage, WaterBody waterBody)
    {
        if (waterContactAction != null)
            waterContactAction.Invoke(damage, waterBody);

    }

}
