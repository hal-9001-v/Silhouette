using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralTools;

public class WaterBody : MonoBehaviour
{

    [Header("References")]
    [SerializeField] LineRenderer[] _lineRenderers;
    [Header("Settings")]
    [SerializeField] [Range(0, 10)] float _dmg;

    private void Awake()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            var del = collider.gameObject.AddComponent<ColliderDelegate>();

            del.TriggerEnterAction += ContactWithWater;
        }

    }

    public Vector3 GetClosestPosition(Vector3 position)
    {
        Vector3 closestPoint = LineMath.GetClosestPointOnLine(_lineRenderers[0], position, false);
        Vector3 newPoint;
        for (int i = 1; i < _lineRenderers.Length; i++)
        {
            newPoint = LineMath.GetClosestPointOnLine(_lineRenderers[i], position, false);

            if (Vector3.Distance(closestPoint, position) > Vector3.Distance(newPoint, position))
            {
                closestPoint = newPoint;
            }
        }

        return closestPoint;

    }

    void ContactWithWater(Transform source, Collider other, Vector3 pos)
    {
        var detector = other.GetComponent<WaterDetector>();

        if (detector != null)
        {
            detector.WaterContact(_dmg, this);
        }
    }


}
