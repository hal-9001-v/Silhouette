using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Blimp : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform _patrolPoint;

    [Header("Settings")]
    [Range(0, 5)]
    [SerializeField] float _lerpFactor = 1;


    Vector3 previousPosition;

    private void Awake()
    {
        if(_patrolPoint != null)
            previousPosition = _patrolPoint.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_patrolPoint != null)
        {
            transform.position = _patrolPoint.position;

            Vector3 direction = transform.position - previousPosition;
            previousPosition = transform.position;

            if (direction != Vector3.zero) {

                Quaternion prevRotation = transform.rotation;
                Quaternion actualRot = Quaternion.LookRotation(direction.normalized);

                var rot = Quaternion.Lerp(prevRotation, actualRot, Time.deltaTime * _lerpFactor).eulerAngles;

                rot.z = 0;
                rot.x = 0;
                transform.eulerAngles = rot;
            }


        }
    }

    private void OnDrawGizmos()
    {

        if (_patrolPoint != null) {
            Gizmos.color = Color.yellow;

            Gizmos.DrawCube(_patrolPoint.position, new Vector3(2,2,2));
        }

    }

}
