using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noiser : MonoBehaviour
{
    MobRegister _mobRegister;

    private void Awake()
    {
        _mobRegister = FindObjectOfType<MobRegister>();
    }

    public void MakeNoise(Vector3 position, float range)
    {
        if (_mobRegister.listeners != null)
        {

            foreach (Listener listener in _mobRegister.listeners)
            {
                listener.Noise(position, range, this);
            }

        }
    }

    public bool canBeHeared(Vector3 position, float range)
    {
        if (_mobRegister.listeners != null)
        {

            foreach (Listener listener in FindObjectsOfType<Listener>())
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
