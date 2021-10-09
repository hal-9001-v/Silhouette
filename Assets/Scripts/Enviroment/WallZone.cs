using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GeneralTools;

public class WallZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider _rightLimit;
    [SerializeField] Collider _leftLimit;

    [Header("Settings")]
    public Vector3 Direction;
    public LineRenderer line;

    public UnityEvent atStartEvents;
    public UnityEvent atEndEvents;

    CharacterWallSneak _player;

    public float CurveLength { get; private set; }

    private void Awake()
    {
        _player = FindObjectOfType<CharacterWallSneak>();

        InitializeLimitColliders();
    }

    void InitializeLimitColliders()
    {
        if (_rightLimit != null)
        {
            ColliderDelegate colliderDelegate = _rightLimit.gameObject.AddComponent<ColliderDelegate>();

            colliderDelegate.TriggerEnterAction += (source, coll, pos) =>
            {
                var other = coll.GetComponent<CharacterWallSneak>();

                if (other != null)
                    _player.atRightLimit = true;
            };

            colliderDelegate.TriggerExitAction += (source, coll, pos) =>
            {
                var other = coll.GetComponent<CharacterWallSneak>();

                if (other != null)
                    _player.atRightLimit = false;
            };

        }

        if (_leftLimit != null)
        {
            ColliderDelegate colliderDelegate = _leftLimit.gameObject.AddComponent<ColliderDelegate>();

            colliderDelegate.TriggerEnterAction += (source, coll, pos) =>
            {
                var other = coll.GetComponent<CharacterWallSneak>();

                if (other != null)
                    _player.atLeftLimit = true;
            };

            colliderDelegate.TriggerExitAction += (source, coll, pos) =>
            {
                var other = coll.GetComponent<CharacterWallSneak>();

                if (other != null)
                    _player.atLeftLimit = false;
            };

        }
    }

    public void StickPlayer()
    {
        _player.StickToWall(this);

        atStartEvents.Invoke();
    }

    public Vector3 GetClosestPoint(Vector3 position)
    {
        return LineMath.GetClosestPointOnLine(line, position, true);
    }
    public int GetClosestIndex(Vector3 position)
    {
        int index = 0;

        for (int i = 1; i < line.positionCount; i++)
        {

            Vector3 newPosition = line.transform.TransformPoint(line.GetPosition(i));
            //Vector3 newPosition = line.GetPosition(i);

            if (Vector3.Distance(position, newPosition) < Vector3.Distance(line.transform.TransformPoint(line.GetPosition(index)), position))
            {
                index = i;
            }
        }
        return index;
    }

    public Vector3 GetPoint(int i)
    {
        if (i >= line.positionCount)
        {
            i = line.positionCount - 1;

        }
        else if (i < 0)
        {
            i = 0;
        }

        return line.transform.TransformPoint(line.GetPosition(i));
    }

    public void UnstickPlayer()
    {
        atEndEvents.Invoke();

        _player.UnstickToWall();
    }



}

public class LinePoint{
    
    public Vector3 position;
    public LinePoint leftPoint;
    public LinePoint rightPoint;

}
