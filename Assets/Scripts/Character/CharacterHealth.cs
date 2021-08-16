using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(Health), typeof(WaterDetector))]
public class CharacterHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] CharacterMovement _characterMovement;
    [SerializeField] CharacterAligner _characterAligner;
    [SerializeField] TextMeshProUGUI _textMesh;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 2)] float _knockUpDuration;
    [SerializeField] [Range(0.1f, 20)] float _waterRecoverSpeed;

    Health _health;
    WaterDetector _waterDetector;

    private void Start()
    {
        _health = GetComponent<Health>();
        _waterDetector = GetComponent<WaterDetector>();

        _health.HurtAction += HurtPlayer;
        _health.DeadAction += KillPlayer;

        _waterDetector.waterContactAction += ContactWithWater;

    }


    void HurtPlayer(Vector3 source, float push, Transform hitter)
    {
        if (_textMesh != null)
        {
            _textMesh.text = _health.CurrentHealth.ToString();
        }

        StartCoroutine(KnockUpCharacter(_knockUpDuration));

        Vector3 pushDirection = _characterMovement.transform.position - source;
        pushDirection.y = pushDirection.magnitude / 3;

        _characterMovement.Push(pushDirection.normalized * push);


    }

    void ContactWithWater(float damage)
    {

        _characterMovement.semaphore.Lock();

        _characterAligner.AlignCharacter(_characterMovement.recoverGround, _waterRecoverSpeed, () =>
        {
            _characterMovement.semaphore.Unlock();
        });
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
