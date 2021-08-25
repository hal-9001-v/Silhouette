using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] [Range(0, 1)] float _groundCheckRadius = 0.2f;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Color _gizmosColor = Color.yellow;

    public bool isGrounded
    {
        get
        {
            if (_groundCheck != null)
            {
                var groundCollisions = Physics.OverlapSphere(_groundCheck.position, _groundCheckRadius, _groundMask);


                if (groundCollisions.Length != 0)
                {
                    if (_rigidbody != null)
                    {
                        if (_rigidbody.velocity.y < 0) return true;
                    }
                    else
                    {
                        return true;
                    }
                }

                /*foreach (Collider c in groundCollisions)
                {
                    return true;
                }
                */
            }

            return false;
        }



    }


    private void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }

}
