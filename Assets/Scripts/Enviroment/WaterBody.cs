using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBody : MonoBehaviour
{
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


    void ContactWithWater(Transform source, Collider other, Vector3 pos)
    {
        var detector = other.GetComponent<WaterDetector>();

        if (detector != null)
        {
            detector.WaterContact(_dmg);
        }
    }


}
