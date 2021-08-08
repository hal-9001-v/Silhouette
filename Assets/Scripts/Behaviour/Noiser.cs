using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noiser : MonoBehaviour
{

    Listener[] _listeners;

    private void Awake()
    {
        _listeners = FindObjectsOfType<Listener>();
    }

    public void MakeNoise(Vector3 position, float range)
    {
        if (_listeners != null)
        {

            foreach (Listener listener in _listeners)
            {
                listener.Noise(position, range, this);
            }

        }
    }

    public bool canBeHeared(Vector3 position, float range)
    {
        if (_listeners != null)
        {

            foreach (Listener listener in _listeners)
            {
                if (listener.CanHearNoise(position, range))
                {
                    return true;
                }
            }

        }

        return false;
    }

}
