using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAligner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] CharacterAnimationCommand _animationCommand;

    [Header("Settings")]
    [SerializeField]
    [Range(0.01f, 0.1f)] float _tolerance;

    //Aligment
    Vector3 _targetPosition;
    Vector3 _startingPosition;

    bool _isAligning;
    float _totalTime;
    float _elapsedTime;

    Action _alignmentCallback;

    public void AlignCharacter(Vector3 targetPosition, float speed, Action callback)
    {
        if (_rigidbody != null)
        {

            _rigidbody.useGravity = false;
            _isAligning = true;

            _targetPosition = targetPosition;
            _startingPosition = transform.position;
            _alignmentCallback = callback;

            _totalTime = Vector3.Distance(_startingPosition, targetPosition) / speed;
            _elapsedTime = 0;

            _rigidbody.detectCollisions = false;


            if (_animationCommand != null)
            {
                _animationCommand.Align();

                _alignmentCallback += _animationCommand.StopAlign;
            }
        }

    }

    void StopAlign()
    {
        _isAligning = false;
        _rigidbody.detectCollisions = true;
        _rigidbody.useGravity = true;

        if (_alignmentCallback != null)
            _alignmentCallback.Invoke();
    }

    void CalculateAlignment()
    {
        _elapsedTime += Time.fixedDeltaTime;

        transform.position = Vector3.Lerp(_startingPosition, _targetPosition, _elapsedTime / _totalTime);
    }

    private void FixedUpdate()
    {
        if (_isAligning)
        {
            if (_totalTime <= _elapsedTime)
            {
                StopAlign();
            }
            else
            {
                CalculateAlignment();
            }
        }
    }

}
