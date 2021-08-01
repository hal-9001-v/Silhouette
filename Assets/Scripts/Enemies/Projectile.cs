using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] UnityEvent _onHitPlayer;
    [SerializeField] UnityEvent _onHit;

    //References
    Rigidbody _rigidBody;
    Collider _collider;
    Renderer _renderer;

    float _damage = 1;
    float _push = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _renderer = GetComponent<Renderer>();


        //Nullify parent so it doesn't move with hierarchy on any way.
        transform.parent = null;

        Hide();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            health.Hurt(_damage, transform.position, _push, transform);

            _onHitPlayer.Invoke();
        }

        _onHit.Invoke();
        Hide();

    }

    /// <summary>
    /// Shoot Projectile from position
    /// </summary>
    /// <param name="startingPosition"></param>
    /// <param name="velocity"></param>
    public void Launch(Vector3 startingPosition, Vector3 velocity, float damage, float push)
    {
        _damage = damage;
        _push = push;

        transform.position = startingPosition;
        _rigidBody.velocity = velocity;

        Show();
    }

    /// <summary>
    /// Shoot projectile towards Target from position
    /// </summary>
    /// <param name="startingPosition"></param>
    /// <param name="target"></param>
    public void Launch(Vector3 startingPosition, float velocity, float damage, float push, Transform target)
    {
        Launch(startingPosition, (target.position - startingPosition).normalized * velocity, damage, push);
    }

    /// <summary>
    /// Enable Colliders and Renderers.
    /// </summary>
    public void Show()
    {
        _collider.enabled = true;
        _renderer.enabled = true;
    }

    /// <summary>
    /// Disable Colliders and Renderers.
    /// </summary>
    public void Hide()
    {
        _collider.enabled = false;
        _renderer.enabled = false;

        _rigidBody.velocity = Vector3.zero;
    }

}
