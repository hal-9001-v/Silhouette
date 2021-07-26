using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidgeonDestination : MonoBehaviour
{
    [Header("Gizmos Settings")]
    [Range(0, 20)]
    [SerializeField] float _radius = 5;
    [Range(0, 20)]
    [SerializeField] Color _color = Color.red;



    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }

}
