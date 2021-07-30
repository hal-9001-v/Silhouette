using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator _animator;
    [Header("Settings")]
    [Range(0, 30)]
    [SerializeField] float _jumpHeight = 10;
    [SerializeField] TypeOfJumper _typeOfJumper;

    enum TypeOfJumper
    {
        Upper,
        Direction
    }

#if UNITY_EDITOR
    [Header("Gizmos")]
    [Range(0, 2)]
    [SerializeField] float _gizmosWidth = 1;
    [SerializeField] Color _gizmosColor;
#endif

    CharacterMovement _player;

    //Animator Variables
    const string _jumpTrigger = "Jump";

    private void Awake()
    {
        _player = FindObjectOfType<InteractionTrigger>().GetComponent<CharacterMovement>();
    }

    public void LaunchPlayer()
    {

        if (_player != null)
        {

            switch (_typeOfJumper)
            {
                case TypeOfJumper.Upper:
                    _player.Launch(_jumpHeight);
                    break;

                case TypeOfJumper.Direction:
                    Vector3 jumpVelocity = Vector3.zero;

                    float launchMagnitude = 2 * Mathf.Abs(Physics.gravity.y) * _jumpHeight;

                    launchMagnitude = Mathf.Pow(launchMagnitude, 0.5f);

                    jumpVelocity.y = launchMagnitude;

                    _player.Launch(transform.TransformDirection(jumpVelocity));

                    break;
            }

            if (_animator != null)
            {
                _animator.SetTrigger(_jumpTrigger);
            }


        }

    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;


        var cubePosition = transform.position;
        cubePosition += new Vector3(0, 0.5f * _jumpHeight, 0);


        Gizmos.DrawWireCube(cubePosition,new Vector3(_gizmosWidth, _jumpHeight, _gizmosWidth));
    }
#endif


}
