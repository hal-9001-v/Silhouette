using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuable : MonoBehaviour
{
    public Rooftop currentRooftop { get; set; }

    public bool isOnRooftop
    {
        get
        {
            return currentRooftop != null;
        }
    }

}
