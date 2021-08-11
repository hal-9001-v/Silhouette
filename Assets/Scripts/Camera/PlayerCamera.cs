using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class PlayerCamera : InputComponent
{
    [Header("References")]
    [SerializeField] Transform _cameraFollow;
    [SerializeField] Transform _body;
    [SerializeField] CinemachineVirtualCamera _characterCamera;


    public CinemachineVirtualCamera activeCamera { get; private set; }
    CinemachineVirtualCamera _previousCamera;

    public TypeOfActiveCamera typeOfCamera { get; private set; }
    TypeOfActiveCamera _previousType;

    float _firstPersonX;

    [Header("Settings")]
    [Range(0.1f, 1)]
    [SerializeField] float _mouseSensitivity;

    [Range(0.1f, 10)]
    [SerializeField] float _padSensitivity;

    public enum TypeOfActiveCamera
    {
        Player,
        Fixed,
        Vent,
        Binocucom
    }

    CinemachineVirtualCamera[] _cameras;
    Vector2 _inputMouseRotation;
    Vector2 _inputPadRotation;


    private void Awake()
    {
        _cameras = FindObjectsOfType<CinemachineVirtualCamera>();

        SetActiveCamera(_characterCamera, TypeOfActiveCamera.Player);
    }

    public void SetActiveCamera(CinemachineVirtualCamera newActiveCamera, TypeOfActiveCamera type)
    {
        if (activeCamera != null)
        {
            _previousCamera = activeCamera;
            _previousType = typeOfCamera;

        }
        else
        {
            _previousCamera = newActiveCamera;
            _previousType = type;
        }
        _firstPersonX = 0;

        activeCamera = newActiveCamera;
        typeOfCamera = type;

        foreach (CinemachineVirtualCamera camera in _cameras)
        {
            camera.enabled = false;
        }

        newActiveCamera.enabled = true;

        /*
        switch (type)
        {
            case TypeOfActiveCamera.Binocucom:
                newActiveCamera.transform.position = _binocucomPosition.position;
                break;

            case TypeOfActiveCamera.Vent:
                newActiveCamera.transform.position = _ventBinocucomPosition.position;
                break;

            default:
                break;
        }
        */
    }

    public Vector3 GetForward()
    {
        switch (typeOfCamera)
        {
            case TypeOfActiveCamera.Player:
                return _cameraFollow.forward;
            //break;

            case TypeOfActiveCamera.Fixed:
                return activeCamera.transform.forward;
            //break;

            case TypeOfActiveCamera.Vent:
                return activeCamera.transform.forward;
            //break;

            default:
                return Vector3.zero;
                //break;
        }
    }

    public Vector3 GetRight()
    {
        switch (typeOfCamera)
        {
            case TypeOfActiveCamera.Player:
                return _cameraFollow.right;
            //break;

            case TypeOfActiveCamera.Fixed:
                return activeCamera.transform.right;
            //break;

            case TypeOfActiveCamera.Vent:
                return activeCamera.transform.right;
            //break;

            default:
                return Vector3.zero;
                //break;

        }
    }

    public void ResetCamera()
    {
        SetActiveCamera(_characterCamera, TypeOfActiveCamera.Player);
    }

    public void ResetCameraToPrevious()
    {
        SetActiveCamera(_previousCamera, _previousType);
    }

    // Start is called before the first frame update
    void Start()
    {
        _inputMouseRotation = new Vector2();
        _inputPadRotation = new Vector2();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        switch (typeOfCamera)
        {
            case TypeOfActiveCamera.Player:
                PlayerCameraRotation();
                break;

            case TypeOfActiveCamera.Binocucom:
                BinocucomRotation();
                break;
            case TypeOfActiveCamera.Vent:
                VentCameraRotation();
                break;

            default:
                break;
        }
    }

    void PlayerCameraRotation()
    {
        if (_cameraFollow != null && (_inputMouseRotation != Vector2.zero || _inputPadRotation != Vector2.zero))
        {
            Quaternion nextRotation = _cameraFollow.transform.rotation;

            nextRotation *= Quaternion.AngleAxis(-(_inputMouseRotation.y * _mouseSensitivity + _inputPadRotation.y * _padSensitivity), Vector3.right);
            nextRotation *= Quaternion.AngleAxis(_inputMouseRotation.x * _mouseSensitivity + +_inputPadRotation.x * _padSensitivity, Vector3.up);

            // _cameraFollow.rotation = Quaternion.Lerp(_cameraFollow.transform.rotation, nextRotation, Time.deltaTime * 1);
            _cameraFollow.rotation = nextRotation;

            var aux = _cameraFollow.eulerAngles;
            aux.z = 0;

            if (aux.x > 180 && aux.x < 340)
            {
                aux.x = 340;
            }
            else if (aux.x < 180 && aux.x > 40)
            {
                aux.x = 40;
            }

            _cameraFollow.eulerAngles = aux;


        }
    }

    void VentCameraRotation()
    {
        if (_body != null && activeCamera != null)
        {/*
            //Rotate Camera on Vertical Axis
            var rotation = Vector3.zero;
            rotation.x = -_inputRotation.y * _mouseSensitivity;
            activeCamera.transform.Rotate(rotation);

            //Clamp Camera on Vertical Axis
            rotation = activeCamera.transform.localEulerAngles;
            rotation.x = Mathf.Clamp(rotation.x, 30, 150);
            activeCamera.transform.localEulerAngles = rotation;*/

            activeCamera.transform.forward = _body.forward;
        }
    }

    void BinocucomRotation()
    {
        if (_body != null && activeCamera != null && (_inputMouseRotation != Vector2.zero || _inputPadRotation != Vector2.zero))
        {

            //Rotate Player towards direction
            float rotation;
            rotation = _inputMouseRotation.x * _mouseSensitivity + _inputPadRotation.x * _padSensitivity;

            //_body.Rotate(Quaternion.AngleAxis(rotation, Vector3.up));

            //Rotate Camera on Vertical Axis
            _firstPersonX -= _inputMouseRotation.y * _mouseSensitivity + _inputPadRotation.y * _padSensitivity;
            //activeCamera.transform.Rotate(rotation);

            //Clamp Camera on Vertical Axis
            //rotation = activeCamera.transform.eulerAngles;
            //rotation.x = Mathf.Clamp(rotation.x, -40, 40);

            _firstPersonX = Mathf.Clamp(_firstPersonX, -40, 40);

            activeCamera.transform.rotation = Quaternion.Euler(_firstPersonX, activeCamera.transform.eulerAngles.y + rotation, 0);

        }
    }

    public override void SetInput(PlatformMap input)
    {
        input.Character.MouseCamera.performed += ctx =>
        {
            _inputMouseRotation = ctx.ReadValue<Vector2>();

        };

        input.Character.MouseCamera.canceled += ctx =>
        {
            _inputMouseRotation = Vector2.zero;
        };

        input.Character.PadCamera.performed += ctx =>
        {
            _inputPadRotation = ctx.ReadValue<Vector2>();

        };

        input.Character.PadCamera.canceled += ctx =>
        {
            _inputPadRotation = Vector2.zero;
        };

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(_ventBinocucomPosition.position, 0.1f);
    }
}
