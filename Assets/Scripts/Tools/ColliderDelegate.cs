using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDelegate : MonoBehaviour
{
    public Action<Collision, Vector3> CollisionEnterAction;
    public Action<Collision, Vector3> CollisionStayAction;
    public Action<Collision, Vector3> CollisionExitAction;
    
    public Action<Collider, Vector3> TriggerEnterAction;
    public Action<Collider, Vector3> TriggerStayAction;
    public Action<Collider, Vector3> TriggerExitAction;


    private void OnCollisionEnter(Collision collision)
    {
        if (CollisionEnterAction != null)
        {
            CollisionEnterAction.Invoke(collision,transform.position);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (CollisionStayAction != null) {
            CollisionStayAction.Invoke(collision, transform.position);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (CollisionExitAction != null)
        {
            CollisionExitAction.Invoke(collision, transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TriggerEnterAction != null)
        {
            TriggerEnterAction.Invoke(other, transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (TriggerStayAction != null)
        {
            TriggerStayAction.Invoke(other, transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TriggerExitAction != null)
        {
            TriggerExitAction.Invoke(other, transform.position);
        }
    }


}
