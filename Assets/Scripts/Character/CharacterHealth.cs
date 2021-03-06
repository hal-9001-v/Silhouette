using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Health), typeof(WaterDetector))]
public class CharacterHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] CharacterMovement _characterMovement;
    [SerializeField] CharacterAligner _characterAligner;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 2)] float _knockUpDuration;
    [SerializeField] [Range(0.1f, 20)] float _waterRecoverSpeed;
    [SerializeField] [Range(0f, 1)] float _hurtRumble;
    [SerializeField] [Range(0f, 5)] float _hurtRumbleDuration;
    [SerializeField] [Range(0f, 1)] float _dieRumble;
    [SerializeField] [Range(0f, 5)] float _dieRumbleDuration;

    UICommand _uiCommand;

    Health _health;
    WaterDetector _waterDetector;
    Rumbler _rumbler;

    public CheckPoint activeCheckPoint;

    private void Start()
    {
        _health = GetComponent<Health>();
        _waterDetector = GetComponent<WaterDetector>();

        _health.hurtAction += HurtPlayer;
        _health.deadAction += KillPlayer;

        _waterDetector.waterContactAction += ContactWithWater;

        _rumbler = FindObjectOfType<Rumbler>();

        _uiCommand = FindObjectOfType<UICommand>();
    }


    void HurtPlayer(Vector3 source, float push, Transform hitter)
    {
        if (_uiCommand)
        {
            _uiCommand.SetHealth(_health.currentHealth);
        }

        StartCoroutine(KnockUpCharacter(_knockUpDuration));

        Vector3 pushDirection = _characterMovement.transform.position - source;
        pushDirection.y = pushDirection.magnitude / 3;

        _characterMovement.Push(pushDirection.normalized * push);

        _rumbler.Rumble(_hurtRumble, _hurtRumble, _hurtRumbleDuration);
    }

    void ContactWithWater(float damage, WaterBody waterBody)
    {

        _characterMovement.semaphore.Lock();

        _characterAligner.AlignCharacter(waterBody.GetClosestPosition(transform.position), _waterRecoverSpeed, () =>
        {
            _characterMovement.semaphore.Unlock();
        });
    }

    IEnumerator KnockUpCharacter(float duration)
    {
        _characterMovement.semaphore.Lock();
        _health.canGetHurt = false;
        yield return new WaitForSeconds(duration);
        _health.canGetHurt = true;

        _characterMovement.semaphore.Unlock();
    }

    void KillPlayer(Vector3 source, float push, Transform hitter)
    {
        if (_uiCommand != null)
        {
            _uiCommand.SetHealth(0);
        }

        _rumbler.Rumble(_dieRumble, _dieRumble, _dieRumbleDuration);

        if (activeCheckPoint == null)
        {
            activeCheckPoint = CheckPoint.GetDefaultCheckPoint();
        }
            transform.position = activeCheckPoint.transform.position;

    }


}
