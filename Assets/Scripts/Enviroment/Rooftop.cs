using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralTools;


public class Rooftop : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider[] _rooftopTriggers;

    [Space(5)]
    [SerializeField] Collider[] _groundTriggers;
    [SerializeField] LineRenderer[] _groundLines;

    [Space(5)]
    [SerializeField] LineRenderer[] _roofLines;

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

        foreach (var collision in _groundTriggers)
        {
            var del = collision.gameObject.AddComponent<ColliderDelegate>();

            del.TriggerStayAction += JumpToRoof;
        }
    }

    public Vector3 GetClosestRoofPoint(Vector3 position, bool transformToWorldSpace)
    {
        return LineMath.GetClosestPointOnLines(_roofLines, position, transformToWorldSpace);
    }

    public Vector3 GetClosestGroundPoint(Vector3 position, bool transformToWorldSpace)
    {
        return LineMath.GetClosestPointOnLines(_groundLines, position, transformToWorldSpace);
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
    /*
    [ContextMenu("Update Patrol Points")]
    void TranslateLinesToLocal()
    {
        if (_fallLines != null && _fallLines.Length != 0)
        {
            foreach (LineRenderer line in _fallLines)
            {

                Vector3[] points = new Vector3[line.positionCount];

                line.GetPositions(points);

                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = transform.InverseTransformPoint(points[i]);
                }

                line.SetPositions(points);
            }
        }

        if (_jumpLines != null && _jumpLines.Length != 0)
        {
            foreach (LineRenderer line in _jumpLines)
            {

                Vector3[] points = new Vector3[line.positionCount];

                line.GetPositions(points);

                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = transform.InverseTransformPoint(points[i]);
                }

                line.SetPositions(points);
            }
        }
    }
    */
}


