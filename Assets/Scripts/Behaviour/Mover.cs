using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("References")]

    [Space(5)]
    [SerializeField] Transform _groundCheck;

    [Space(5)]
    [Header("Settings")]

    //[Range(1, 10)]
    //[SerializeField] float _inAirSpeed = 3;

    [Range(0.01f, 1)]
    [SerializeField] float _ledgeGrabRange;


    [Range(0, 90)]
    [SerializeField] float _ledgeGrabAngle;

    [Range(0.1f, 1)]
    [SerializeField]
    float _groundCheckRadius = 0.2f;

    [SerializeField]
    LayerMask _groundMask;

    public Semaphore semaphore;

    private void Awake()
    {
        semaphore = new Semaphore();
    }


    public bool isGrounded
    {
        get
        {
            if (_groundCheck != null)
            {
                var groundCollisions = Physics.OverlapSphere(_groundCheck.position, _groundCheckRadius, _groundMask);


                if (groundCollisions.Length != 0)
                    return true;

                /*foreach (Collider c in groundCollisions)
                {
                    return true;
                }
                */
            }

            return false;
        }



    }

    public void MovePlayer(Vector3 direction, float speed, Rigidbody rigidbody, ForceMode mode)
    {
        direction.y = 0;
        direction.Normalize();
        direction *= speed;

        //_rigidbody.AddForce(totalVelocity, ForceMode.VelocityChange);

        direction = direction - rigidbody.velocity;

        direction.y = 0;

        rigidbody.AddForce(direction, mode);
    }

    /// <summary>
    /// Launch character on Up direction to specified height
    /// </summary>
    /// <param name="height"></param>
    public void LaunchUp(float height, Rigidbody rigidbody)
    {
        Vector3 jumpVelocity = Vector3.zero;

        float launchMagnitude = 2 * Mathf.Abs(Physics.gravity.y) * height;

        launchMagnitude = Mathf.Pow(launchMagnitude, 0.5f);

        jumpVelocity.y = launchMagnitude;

        Launch(jumpVelocity, rigidbody);
    }

    public void Push(Vector3 velocity, Rigidbody rigidbody)
    {
        Launch(velocity, rigidbody);
    }

    void Launch(Vector3 velocity, Rigidbody rigidbody)
    {
        var currentVerticalVelocity = rigidbody.velocity;
        currentVerticalVelocity.x = 0;
        currentVerticalVelocity.z = 0;
        rigidbody.AddForce(velocity - currentVerticalVelocity, ForceMode.VelocityChange);

    }


}
