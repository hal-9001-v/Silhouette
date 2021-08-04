using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Binocucom : InputComponent
{
    [Header("References")]
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] CinemachineVirtualCamera _camera;

    [SerializeField] CharacterMovement _characterMovement;
    [SerializeField] CharacterHook _characterHook;
    [SerializeField] CharacterHook _characterMelee;
    [SerializeField] CharacterHook _characterWallSneak;

    PlayerCamera _playerCamera;

    bool _displayed;

    private void Awake()
    {
        _playerCamera = FindObjectOfType<PlayerCamera>();
        
    }

    void Start()
    {
        HideBinocucom();
    }

    public void DisplayBinocucom()
    {
        if (_canvasGroup != null && _playerCamera != null && _camera != null)
        {
            _canvasGroup.alpha = 1;
            _displayed = true;

            _playerCamera.SetActiveCamera(_camera, PlayerCamera.TypeOfActiveCamera.Binocucom);

            if (_characterMovement != null) _characterMovement.Lock();
            if (_characterHook != null) _characterHook.Lock();
            if (_characterMelee != null) _characterMelee.Lock();
            if (_characterWallSneak != null) _characterWallSneak.Lock();
        }
    }

    public void HideBinocucom()
    {
        if (_canvasGroup != null && _playerCamera != null)
        {
            _canvasGroup.alpha = 0;
            _displayed = false;

            _playerCamera.ResetCamera();

            if (_characterMovement != null) _characterMovement.Unlock();
            if (_characterHook != null) _characterHook.Lock();
            if (_characterMelee != null) _characterMelee.Lock();
            if (_characterWallSneak != null) _characterWallSneak.Lock();
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
    }
}
