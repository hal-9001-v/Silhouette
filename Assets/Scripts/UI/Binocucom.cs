using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Binocucom : InputComponent
{
    [Header("References")]
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] Transform _standCameraPosition;
    [SerializeField] Transform _crawlCameraPosition;

    [SerializeField] CharacterMovement _characterMovement;
    [SerializeField] CharacterHook _characterHook;
    [SerializeField] CharacterHook _characterMelee;
    [SerializeField] CharacterWallSneak _characterWallSneak;
    [SerializeField] CharacterVent _characterVent;
    [SerializeField] CharacterBodyRotation _characterBodyRotation;

    [Header("Settings")]
    [SerializeField] [Range(0f, 100)] float _minFOV = 30;
    [SerializeField] [Range(0f, 100)] float _maxFOV = 80;
    [SerializeField] [Range(0f, 5)] float _scrollChange = 80;
    PlayerCamera _playerCamera;

    UICommand _uiCommand;

    bool _displayed;

    private void Awake()
    {
        _playerCamera = FindObjectOfType<PlayerCamera>();
        _uiCommand = FindObjectOfType<UICommand>();

    }

    void Start()
    {
        HideBinocucom();
    }

    public void DisplayBinocucom()
    {

        if (_playerCamera != null)
        {
            if (_uiCommand)
            {
                _uiCommand.DisplayBinocucom();
            }
            _displayed = true;


            if (_characterVent.isOnVent) _camera.transform.position = _crawlCameraPosition.position;
            else _camera.transform.position = _standCameraPosition.position;

            _camera.m_Lens.FieldOfView = _maxFOV;

            var previousForward = _playerCamera.GetForward();
            _playerCamera.SetActiveCamera(_camera, PlayerCamera.TypeOfActiveCamera.Binocucom);
            _playerCamera.SetForward(previousForward);

            if (_characterMovement != null) _characterMovement.semaphore.Lock();
            if (_characterHook != null) _characterHook.semaphore.Lock();
            if (_characterMelee != null) _characterMelee.semaphore.Lock();
            if (_characterWallSneak != null) _characterWallSneak.semaphore.Lock();
        }
    }

    void HideBinocucom()
    {
        if (_playerCamera != null)
        {
            if (_uiCommand != null)
            {
                _uiCommand.HideBinocucom();
            }
            _displayed = false;


            var previousForward = _playerCamera.GetForward();
            _playerCamera.ResetCameraToPrevious();
            _playerCamera.SetForward(previousForward);

            _characterBodyRotation.SetForward(previousForward);

            if (_characterMovement != null) _characterMovement.semaphore.Unlock();
            if (_characterHook != null) _characterHook.semaphore.Unlock();
            if (_characterMelee != null) _characterMelee.semaphore.Unlock();
            if (_characterWallSneak != null) _characterWallSneak.semaphore.Unlock();
        }
    }

    public void SightScroll(bool augment)
    {
        if (_displayed)
        {

            if (augment)
            {
                if (_camera.m_Lens.FieldOfView < _maxFOV)
                {
                    _camera.m_Lens.FieldOfView += _scrollChange;
                }

            }
            else
            {

                if (_camera.m_Lens.FieldOfView > _minFOV)
                {
                    _camera.m_Lens.FieldOfView -= _scrollChange;
                }
            }

        }
    }

    public override void SetInput(PlatformMap input)
    {
        input.Character.Binocucom.performed += ctx =>
        {
            if (_displayed)
            {
                HideBinocucom();
            }
            else
            {
                DisplayBinocucom();
            }

        };

        input.Character.BinocucomScroll.performed += ctx =>
        {
            if (ctx.ReadValue<float>() > 0)
            {
                SightScroll(false);
            }
            else
            {
                SightScroll(true);
            }

        };
    }
}
