using System.Collections.Generic;
using UnityEngine;

public class Sighter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Light _spotLight;

    public Light SpotLight
    {
        get
        {
            if (_spotLight != null)
                return _spotLight;
            else
            {
                Debug.LogWarning("No spotlight in " + name + "'s sighter");
                return null;
            }

        }

    }

    [Header("Settings")]
    [Tooltip("Masks that can hide player")]
    [SerializeField] LayerMask _rayCastBlockingMask;

    public static bool IsPointOnSight(Vector3 point, Vector3 len, Vector3 lenForward, float range, float angle, bool rayCast, LayerMask mask)
    {
        Vector3 lenToPoint = point - len;

        if (lenToPoint.magnitude > range) return false;

        //Check if angle between "Forward" of light and light-point is lesser than angle variable
        if (Vector3.Angle(lenToPoint, lenForward) * 2 > angle) return false;

        Debug.DrawLine(point, len, Color.blue);

        if (rayCast)
        {
            RaycastHit hit;
            //Point is not On sight if a collider blocks the way
            if (Physics.Raycast(len, lenToPoint, out hit, lenToPoint.magnitude, mask))
            {
                //Debug.Log(hit.collider.name);
                return false;
            }

        }

        return true;
    }

    public static bool AnyPointOnSight(Vector3[] points, Vector3 len, Vector3 lenForward, float range, float angle, bool rayCast, LayerMask mask)
    {

        foreach (Vector3 point in points)
        {
            if (IsPointOnSight(point, len, lenForward, range, angle, rayCast, mask)) return true;
        }

        return false;

    }

    public SightTrigger GetTargetOnSight()
    {
        foreach (SightTrigger trigger in FindObjectsOfType<SightTrigger>())
        {
            if (trigger.canBeSeen && AnyPointOnSight(trigger.GetSightPoints(), _spotLight.transform.position, _spotLight.transform.forward, _spotLight.range, _spotLight.spotAngle, true, _rayCastBlockingMask))
            {
                return trigger;
            }
        }

        return null;

    }

    /// <summary>
    /// Invoke spottedAction on sightTrigger if on Sight
    /// </summary>
    /// <param name="spot"></param>
    /// <returns></returns>
    public bool IsAnyTargetOnSight(bool spot)
    {
        var trigger = GetTargetOnSight();
        if (trigger)
        {
            if (spot)
            {
                if (trigger.spottedAction != null)
                    trigger.spottedAction.Invoke();
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public SightTrigger[] GetTargetsOnSight()
    {
        List<SightTrigger> sightedTargets = new List<SightTrigger>();
        foreach (SightTrigger trigger in FindObjectsOfType<SightTrigger>())
        {
            if (AnyPointOnSight(trigger.GetSightPoints(), _spotLight.transform.position, _spotLight.transform.forward, _spotLight.range, _spotLight.spotAngle, true, _rayCastBlockingMask))
            {
                sightedTargets.Add(trigger);
            }
        }

        return sightedTargets.ToArray();
    }

}

