using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class StretchZone : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 direction;


    [Header("Gizmos")]
    [SerializeField] [Range(0.1f, 1)] float _arrowRadius;

    [SerializeField] Color _arrowHeadColor = Color.blue;
    [SerializeField] Color _arrowBaseColor = Color.red;
    [SerializeField] Color _arrowLineColor = Color.red;

    private void OnDrawGizmos()
    {
        Gizmos.color = _arrowBaseColor;
        Gizmos.DrawSphere(transform.position, _arrowRadius * 0.5f);

        Gizmos.color = _arrowHeadColor;
        Gizmos.DrawSphere(transform.position + direction.normalized, _arrowRadius);

        Gizmos.color = _arrowLineColor;
        Gizmos.DrawLine(transform.position, transform.position + direction.normalized);

    }
}
