using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class WallZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider _rightLimit;
    [SerializeField] Collider _leftLimit;

    [Header("Settings")]
    public Vector3 Direction;
    public LineRenderer line;

    public UnityEvent AtStartEvents;
    public UnityEvent AtEndEvents;

    [SerializeField] [Range(0.01f, 0.5f)] float _timeStep;
    [SerializeField] [Range(0, 3)] int _betweenPoints;


    private float _currentTime;
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

        AtStartEvents.Invoke();
    }

    public Vector3 GetClosestPoint(Vector3 position)
    {
        Vector3 closestPosition = line.transform.TransformPoint(line.GetPosition(0));

        for (int i = 1; i < line.positionCount; i++)
        {

            Vector3 newPosition = line.transform.TransformPoint(line.GetPosition(i));
            //Vector3 newPosition = line.GetPosition(i);

            if (Vector3.Distance(position, newPosition) < Vector3.Distance(closestPosition, position))
            {
                closestPosition = newPosition;
            }


        }
        return closestPosition;
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


    public Vector3 GetPoint(ref int i)
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
        AtEndEvents.Invoke();

        _player.UnstickToWall();
    }

}
