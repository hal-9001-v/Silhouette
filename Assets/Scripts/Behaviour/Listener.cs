using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Listener : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Range(0, 20)] float _range;

    public float listenRange
    {
        get
        {
            return _range;
        }
    }

    [Header("Gizmos")]
    [SerializeField] bool _gizmos = true;
    [SerializeField] Color _gizmosColor = Color.yellow;

    /// <summary>
    /// Vector 3 is noise position and Transform is noise Source
    /// </summary>
    public Action<Vector3, Noiser> hearedNoiseAction;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">Place where the noise happened.</param>
    /// <param name="noiseRange">Area of sound for a listener to hear. If such point is within listener's area, it will be detected.</param>
    /// <param name="source">Noiser wich made the sound.</param>

    public void Noise(Vector3 position, float noiseRange, Noiser source)
    {
        if (CanHearNoise(position, noiseRange))
        {
            if (hearedNoiseAction != null)
                hearedNoiseAction.Invoke(position, source);
        }

    }

    public bool CanHearNoise(Vector3 position, float noiseRange)
    {
        Vector3 closestNoisePosition;
        if (noiseRange > 0)
        {
            var direction = position - transform.position;
            closestNoisePosition = direction * noiseRange;

        }
        else
        {
            closestNoisePosition = position;
        }


        if (Vector3.Distance(transform.position, closestNoisePosition) <= _range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        if (_gizmos)
        {
            Gizmos.color = _gizmosColor;

            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }

}
