using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    Rigidbody _rigidbody;
    PlayerCamera _playerCamera;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerCamera = FindObjectOfType<PlayerCamera>();

    }

    public void Move(Vector2 input, float speed)
    {
        if (input != Vector2.zero)
        {
            Vector3 velocity = _playerCamera.GetRight() * input.x + _playerCamera.GetForward() * input.y;
            velocity.y = 0;

            Move(velocity.normalized * speed);
        }
        else
        {
            StopMovement();
        }
    }

    public void Move(Vector3 velocity)
    {
        if (_rigidbody != null)
        {
            #region Apply movement in camera Direction

            velocity = velocity - _rigidbody.velocity;
            velocity.y = 0;

            _rigidbody.AddForce(velocity, ForceMode.VelocityChange);

            #endregion


        }
    }

    public void StopMovement()
    {
        if (_rigidbody != null)
        {
            var velocity = _rigidbody.velocity;

            velocity.x = 0;
            velocity.z = 0;

            _rigidbody.velocity = velocity;
        }
    }


    /// <summary>
    /// Launch character on Up direction to specified height
    /// </summary>
    /// <param name="height"></param>
    public void LaunchUp(float height)
    {
        Vector3 jumpVelocity = Vector3.zero;

        float launchMagnitude = 2 * Mathf.Abs(Physics.gravity.y) * height;

        launchMagnitude = Mathf.Pow(launchMagnitude, 0.5f);

        jumpVelocity.y = launchMagnitude;

        Launch(jumpVelocity);

    }

    public void Push(Vector3 velocity)
    {
        Launch(velocity);

    }

    public void Launch(Vector3 velocity)
    {
        var currentVerticalVelocity = _rigidbody.velocity;
        currentVerticalVelocity.x = 0;
        currentVerticalVelocity.z = 0;
        _rigidbody.AddForce(velocity - currentVerticalVelocity, ForceMode.VelocityChange);

    }


}
