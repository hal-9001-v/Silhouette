using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    CheckPointTracker _tracker;

    [Header("References")]
    [SerializeField] Animator _animator;
    [SerializeField] MeshRenderer _hatRenderer;
    [SerializeField] Transform _spawn;

    public Vector3 SpawnPosition
    {
        get
        {
            return _spawn.position;
        }
    }

    //Animator Variables
    const string appearTrigger = "Appear";
    const string disappearTrigger = "Disappear";


    private void Start()
    {
        _tracker = FindObjectOfType<CheckPointTracker>();

        if (_tracker == null)
            Debug.LogWarning("No CheckPointTracker in Scene!");

        if (_hatRenderer != null)
        {
            _hatRenderer.enabled = false;
        }
    }

    public void SetCheckPoint()
    {

        if (_tracker != null && _tracker.currentCheckPoint != this)
        {

            _tracker.SetCheckPoint(this);


            _animator.SetTrigger(appearTrigger);

            if (_hatRenderer != null)
            {
                _hatRenderer.enabled = true;
            }


        }
    }

    public void DisableCheckPoint(Component other)
    {
        if (other.GetType() == typeof(CheckPointTracker))
        {

            if (_animator != null)
            {
                _animator.SetTrigger(disappearTrigger);
            }

            if (_hatRenderer != null)
            {
                _hatRenderer.enabled = false;
            }

        }

    }


}
