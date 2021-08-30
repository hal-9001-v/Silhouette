using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Collectable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Range(1, 10)] float _movementSpeed = 5;
    [SerializeField] [Range(0.1f, 1)] float _moveDistance = 0.3f;
    [SerializeField] ReachedEffect _reachedEffect;

    [Space(5)]
    [SerializeField] UnityEvent _collectedUnityEvent;


    enum ReachedEffect
    {
        Destroy,
        Disappear,
        Nothing
    }

    public Action<Inventory> collectedAction;

    bool _triggered;

    Inventory _targetInventory;

    Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.GetComponent<Inventory>();

        if (inventory)
            Collect(inventory);
    }

    public void Collect(Inventory inventory)
    {
        if (_triggered == false)
        {
            //Inventory to add this collectable
            _targetInventory = inventory;

            //Start Closening object to player in Fixed Update
            _triggered = true;

            //Disable Colliders
            foreach (Collider collider in GetComponents<Collider>())
            {
                collider.enabled = false;
            }

        }
    }


    private void FixedUpdate()
    {
        if (_triggered)
        {
            var desiredVelocity = (_targetInventory.transform.position - _rigidbody.position).normalized * _movementSpeed;

            var velocity = desiredVelocity - _rigidbody.velocity;

            _rigidbody.AddForce(velocity, ForceMode.VelocityChange);

            if (Vector3.Distance(_targetInventory.transform.position, _rigidbody.position) < _moveDistance)
            {
                _triggered = false;

                AddCollectable();
            }
        }


    }

    void AddCollectable()
    {
        _collectedUnityEvent.Invoke();

        if (collectedAction != null)
        {
            collectedAction.Invoke(_targetInventory);
        }

        switch (_reachedEffect)
        {
            case ReachedEffect.Destroy:

                Destroy(gameObject);
                break;

            case ReachedEffect.Disappear:
                foreach (Renderer renderer in GetComponents<Renderer>())
                {
                    renderer.enabled = false;
                }

                foreach (Collider collider in GetComponents<Collider>())
                {
                    collider.enabled = false;
                }

                break;

            case ReachedEffect.Nothing:
                break;
        }

    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

}
