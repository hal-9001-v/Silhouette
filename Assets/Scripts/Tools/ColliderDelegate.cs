using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDelegate : MonoBehaviour
{
    public Action<Collision> CollisionEnterAction;
    public Action<Collision> CollisionStayAction;
    public Action<Collision> CollisionExitAction;
    
    public Action<Collider> TriggerEnterAction;
    public Action<Collider> TriggerStayAction;
    public Action<Collider> TriggerExitAction;


    private void OnCollisionEnter(Collision collision)
    {
        if (CollisionEnterAction != null)
        {
            CollisionEnterAction.Invoke(collision);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (CollisionStayAction != null) {
            CollisionStayAction.Invoke(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (CollisionExitAction != null)
        {
            CollisionExitAction.Invoke(collision);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TriggerEnterAction != null)
        {
            TriggerEnterAction.Invoke(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (TriggerStayAction != null)
        {
            TriggerStayAction.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TriggerExitAction != null)
        {
            TriggerExitAction.Invoke(other);
        }
    }


}
