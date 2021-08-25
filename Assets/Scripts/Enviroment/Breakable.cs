using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class Breakable : MonoBehaviour
{

    [SerializeField] UnityEvent _hitEvent;
    [SerializeField] UnityEvent _breakEvent;

    // Start is called before the first frame update
    void Start()
    {

        var health = GetComponent<Health>();

        if (_hitEvent != null)
            health.HurtAction += HitEventInvoke;

        if (_breakEvent != null)
            health.DeadAction += DeadEventInvoke;

    }

    void DeadEventInvoke(Vector3 pos, float push, Transform source)
    {
        _breakEvent.Invoke();
    }

    void HitEventInvoke(Vector3 pos, float push, Transform source)
    {
        _hitEvent.Invoke();
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
