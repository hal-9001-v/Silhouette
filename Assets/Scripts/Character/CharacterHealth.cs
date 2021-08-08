using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(Health))]
public class CharacterHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] CharacterMovement _characterMovement;
    [SerializeField] TextMeshProUGUI _textMesh;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 2)] float _knockUpDuration;

    Health _health;


    private void Start()
    {
        _health = GetComponent<Health>();

        _health.HurtAction += HurtPlayer;
        _health.DeadAction += KillPlayer;

    }


    void HurtPlayer(Vector3 source, float push, Transform hitter)
    {
        if (_textMesh != null)
        {
            _textMesh.text =_health.CurrentHealth.ToString();
        }

        StartCoroutine(KnockUpCharacter(_knockUpDuration));

        Vector3 pushDirection = _characterMovement.transform.position - source;
        pushDirection.y = pushDirection.magnitude / 3;

        _characterMovement.Push(pushDirection.normalized * push);


    }

    IEnumerator KnockUpCharacter(float duration)
    {
        _characterMovement.semaphore.Lock();
        _health.CanGetHurt = false;
        yield return new WaitForSeconds(duration);
        _health.CanGetHurt = true;

        _characterMovement.semaphore.Unlock();
    }

    void KillPlayer(Vector3 source, float push, Transform hitter)
    {
        if (_textMesh != null)
        {
            _textMesh.text = "X_X R.I.P.";
        }

    }


}
