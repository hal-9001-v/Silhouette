using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WindCollider : MonoBehaviour
{
    Collider[] _colliders;

    public float windValue { get; private set; }

    [Header("Settings")]
    [SerializeField] Vector3 _defaultWindVelocity;
    public Vector3 windVelocity { get; private set; }

    [SerializeField] [Range(0, 5)] float _windChangeSpeed;
    [SerializeField] [Range(0.01f, 0.2f)] float _stepTime = 0.01f;
    float windElapsedTime = 0;



    private void Awake()
    {
        _colliders = GetComponents<Collider>();
    }

    private void FixedUpdate()
    {

        float windTimeFactor = Mathf.Abs(Mathf.Sin(windElapsedTime));

        windElapsedTime += _stepTime * _windChangeSpeed;

        windVelocity = _defaultWindVelocity * windTimeFactor;

    }

    public bool PointIsColliding(Vector3 position)
    {
        foreach (Collider collider in _colliders)
        {
            if (collider.bounds.Contains(position))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (_colliders != null)
        {
            foreach (Collider c in _colliders)
            {
                Gizmos.DrawWireCube(c.bounds.center, c.bounds.size);
            }
        }

    }

}
