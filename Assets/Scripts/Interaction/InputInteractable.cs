using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputInteractable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool _onlyOnce;
    [SerializeField] UnityEvent _pressEvent;
    [SerializeField] UnityEvent _releaseEvent;

    [SerializeField] Condition _condition;
    [SerializeField] [Range(0, 20)] float _distance;

    enum Condition
    {
        Distance,
        Logic
    }

    bool _done;
    public bool Ready;
    bool _pressInteractionCalled;
    Transform _target;

    private void Start()
    {
        _target = FindObjectOfType<InteractionTrigger>().transform;
    }

    public void PressInteraction()
    {
        if (_onlyOnce && _done)
            return;

        bool execute = false; ;

        switch (_condition)
        {
            case Condition.Distance:
                if (_target != null && Vector3.Distance(transform.position, _target.position) < _distance) 
                    execute = true;
                break;

            case Condition.Logic:
                execute = Ready;
                break;

            default:
                break; 

        }

        if (execute) {
            _pressEvent.Invoke();

            _pressInteractionCalled = true;
        }
        
    }


    public void ReleaseInteraction()
    {
        if (_onlyOnce && _done)
            return;

        if (_pressInteractionCalled)
        {
            _done = true;

            _releaseEvent.Invoke();

            //Reset boolean for next PressInteraction
            _pressInteractionCalled = false;

        }

    }

    public void SetLogicReady(bool ready)
    {
        Ready = ready;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _distance);
    }

}
