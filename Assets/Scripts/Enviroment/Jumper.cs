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

#if UNITY_EDITOR
    [Header("Gizmos")]
    [Range(0,2)]
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

    public void LaunchPlayer() { 
        
        if(_player != null){

            _player.LaunchPlayer(_jumpHeight);

            if (_animator != null) {
                _animator.SetTrigger(_jumpTrigger);
            }

        
        }

    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;


        var cubePosition = transform.position;
        cubePosition += new Vector3(0, 0.5f*_jumpHeight, 0);
        

        Gizmos.DrawWireCube(cubePosition , new Vector3(_gizmosWidth, _jumpHeight, _gizmosWidth));
    }
#endif


}
