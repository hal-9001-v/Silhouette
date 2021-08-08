using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RooftopDetector : MonoBehaviour
{
    private void Awake()
    {
        foreach (Collider coll in GetComponentsInChildren<Collider>())
        {
            var colliderDelegate = coll.gameObject.AddComponent<ColliderDelegate>();

            colliderDelegate.TriggerEnterAction += SetOnRooftop;
            colliderDelegate.TriggerExitAction += SetOutOfRooftop;
        }
    }

    void SetOnRooftop(Collider other, Vector3 pos)
    {
        var info = other.GetComponent<EnviromentInfo>();
        if (info != null)
        {
            info.isOnRooftop = true;
        }
    }

    void SetOutOfRooftop(Collider other, Vector3 pos)
    {
        var info = other.GetComponent<EnviromentInfo>();
        if (info != null)
        {
            info.isOnRooftop = false;
        }

    }


}
