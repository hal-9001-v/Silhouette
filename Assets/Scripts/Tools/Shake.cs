using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    float _xShake;

    [SerializeField]
    [Range(0, 1)]
    float _yShake;

    [SerializeField]
    [Range(0, 1)]
    float _zShake;

    [SerializeField]
    [Range(0, 1)]
    float _speed;

    [SerializeField]
    Transform _target;

    Vector3 originalPosition;
    private void Awake()
    {
        originalPosition = _target.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_target != null)
        {

            var sin = Mathf.Sin(Time.deltaTime * _speed);

            _target.position = originalPosition + new Vector3(sin * _xShake, sin * _yShake, sin * _zShake);
        }
    }
}
