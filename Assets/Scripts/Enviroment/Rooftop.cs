using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralTools;


public class Rooftop : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider[] _rooftopTriggers;

    [Space(5)]
    [SerializeField] Collider[] _jumpTriggers;
    [SerializeField] LineRenderer[] _jumpLines;

    [Space(5)]
    [SerializeField] Collider[] _fallTriggers;
    [SerializeField] LineRenderer[] _fallLines;

    private void Awake()
    {
        foreach (var collision in _rooftopTriggers)
        {
            var del = collision.gameObject.AddComponent<ColliderDelegate>();

            del.TriggerEnterAction += SetRooftop;

        }

        foreach (var collision in _rooftopTriggers)
        {
            var del = collision.gameObject.AddComponent<ColliderDelegate>();

            del.TriggerExitAction += SetOutOfRooftop;

        }

        foreach (var collision in _jumpTriggers)
        {
            var del = collision.gameObject.AddComponent<ColliderDelegate>();

            del.TriggerStayAction += JumpToRoof;
        }

        foreach (var collision in _fallTriggers)
        {
            var del = collision.gameObject.AddComponent<ColliderDelegate>();

            del.TriggerStayAction += FallFromRoof;
        }

    }

    public Vector3 GetClosestJumpPoint(Vector3 position)
    {
        return LineMath.GetClosestPointOnLines(_jumpLines, position);
    }

    public Vector3 GetClosestFallPoint(Vector3 position)
    {
        return LineMath.GetClosestPointOnLines(_fallLines, position);
    }
    void JumpToRoof(Transform caller, Collider other, Vector3 position)
    {
        var nav = other.GetComponent<Navigator>();

        if (nav != null)
        {
            nav.JumpIntoRooftop(this);
        }
    }

    void SetRooftop(Transform caller, Collider other, Vector3 position)
    {
        var pursuable = other.GetComponent<Pursuable>();

        if (pursuable != null)
        {
            pursuable.currentRooftop = this;
        }
    }

    void SetOutOfRooftop(Transform caller, Collider other, Vector3 position)
    {
        var pursuable = other.GetComponent<Pursuable>();

        if (pursuable != null)
        {
            pursuable.currentRooftop = null;
        }
    }

    void FallFromRoof(Transform caller, Collider other, Vector3 position)
    {
        var nav = other.GetComponent<Navigator>();

        if (nav != null)
        {
            //nav.(this);
        }
    }

}
