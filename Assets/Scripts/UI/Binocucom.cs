using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Binocucom : InputComponent
{
    [Header("References")]
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] CinemachineVirtualCamera _camera;

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
        }
    }

    public void HideBinocucom()
    {
        if (_canvasGroup != null && _playerCamera != null)
        {
            _canvasGroup.alpha = 0;
            _displayed = false;

            _playerCamera.ResetCamera();
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
