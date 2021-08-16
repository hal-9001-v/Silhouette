using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDelegate : MonoBehaviour
{
    /// <summary>
    /// Transform is this, Collision is other, and Vector3 is the position of this collider at such collision
    /// </summary>
    public Action<Transform, Collision, Vector3> CollisionEnterAction;
    /// <summary>
    /// Transform is this, Collision is other, and Vector3 is the position of this collider at such collision
    /// </summary>
    public Action<Transform, Collision, Vector3> CollisionStayAction;
    /// <summary>
    /// Transform is this, Collision is other, and Vector3 is the position of this collider at such collision
    /// </summary>
    public Action<Transform, Collision, Vector3> CollisionExitAction;
    /// <summary>
    /// Transform is this, Collision is other, and Vector3 is the position of this collider at such collision
    /// </summary>
    public Action<Transform, Collider, Vector3> TriggerEnterAction;
    /// <summary>
    /// Transform is this, Collision is other, and Vector3 is the position of this collider at such collision
    /// </summary>
    public Action<Transform, Collider, Vector3> TriggerStayAction;
    /// <summary>
    /// Transform is this, Collision is other, and Vector3 is the position of this collider at such collision
    /// </summary>
    public Action<Transform, Collider, Vector3> TriggerExitAction;


    private void OnCollisionEnter(Collision collision)
    {
        if (CollisionEnterAction != null)
        {
            CollisionEnterAction.Invoke(transform, collision, transform.position);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (CollisionStayAction != null)
        {
            CollisionStayAction.Invoke(transform, collision, transform.position);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (CollisionExitAction != null)
        {
            CollisionExitAction.Invoke(transform, collision, transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TriggerEnterAction != null)
        {
            TriggerEnterAction.Invoke(transform, other, transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (TriggerStayAction != null)
        {
            TriggerStayAction.Invoke(transform, other, transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TriggerExitAction != null)
        {
            TriggerExitAction.Invoke(transform, other, transform.position);
        }
    }


}
