using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform _stoppingPlace;
    public EndVelocity EndingVelocity;

    public enum EndVelocity { 
        stop,
        keep
    }

    public Transform StoppingPlace
    {
        get
        {
            if (_stoppingPlace == null)
            {
                return transform;
            }
            else
            {
                return _stoppingPlace;
            }
        }

    }
    public void HookCharacter(CharacterHook character)
    {

    }
}
