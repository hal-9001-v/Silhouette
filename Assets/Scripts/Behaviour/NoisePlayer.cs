using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Noiser))]
public class NoisePlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Range(0.1f, 20)] float _range = 4;
    [SerializeField] Color _gizmosColor = Color.yellow;

    Noiser _noiser;

    private void Awake()
    {
        _noiser = GetComponent<Noiser>();
    }

    public void PlayNoise()
    {
        _noiser.MakeNoise(transform.position, _range);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(transform.position, _range);

    }

}
