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
    [SerializeField] CinemachineVirtualCamera _binocucomCamera;

    public CinemachineVirtualCamera ActiveCamera { get; private set; }
    Coroutine _changeInputCoroutine;
    bool _isBlendingCamera;

    public TypeOfActiveCamera TypeOfCamera { get; private set; }

    public Action StartBlendAction;
    public Action EndBlendAction;

    [Header("Settings")]
    [Range(0.1f, 1)]
    [SerializeField] float _mouseSensitivity;

    public enum TypeOfActiveCamera
    {
        Player,
        Fixed,
        Binocucom
    }

    CinemachineVirtualCamera[] _cameras;
    CinemachineBrain _brain;
    Vector2 _inputRotation;

    public override void SetInput(PlatformMap input)
    {
        input.Character.Camera.performed += ctx =>
        {
            _inputRotation = ctx.ReadValue<Vector2>();

        };

        input.Character.Camera.canceled += ctx =>
        {
            _inputRotation = Vector2.zero;
        };


    }

    private void Awake()
    {
        _cameras = FindObjectsOfType<CinemachineVirtualCamera>();
        _brain = FindObjectOfType<CinemachineBrain>();

        SetActiveCamera(_characterCamera, TypeOfActiveCamera.Player);
    }

    public void SetActiveCamera(CinemachineVirtualCamera newActiveCamera, TypeOfActiveCamera type)
    {
        if (!_isBlendingCamera)
        {

            if (StartBlendAction != null)
            {
                StartBlendAction.Invoke();
            }

            ActiveCamera = newActiveCamera;

            float blendDuration = StartCameraBlend(newActiveCamera);


            if (_changeInputCoroutine != null)
            {
                StopCoroutine(_changeInputCoroutine);
            }

            _changeInputCoroutine = StartCoroutine(UpdateCameraInput(blendDuration, type));
        }

    }

    float StartCameraBlend(CinemachineVirtualCamera targetCamera)
    {
        foreach (CinemachineVirtualCamera camera in _cameras)
        {
            camera.enabled = false;
        }

        targetCamera.enabled = true;

        if (_brain.ActiveBlend != null)
            return _brain.ActiveBlend.Duration;
        else
            return 0;

    }

    public Vector3 GetForward()
    {
        switch (TypeOfCamera)
        {
            case TypeOfActiveCamera.Player:
                return _cameraFollow.forward;
            //break;

            case TypeOfActiveCamera.Fixed:
                return ActiveCamera.transform.forward;
            //break;

            default:
                return Vector3.zero;
                //break;
        }
    }

    public Vector3 GetRight()
    {
        switch (TypeOfCamera)
        {
            case TypeOfActiveCamera.Player:
                return _cameraFollow.right;
            //break;

            case TypeOfActiveCamera.Fixed:
                return ActiveCamera.transform.right;
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

    IEnumerator UpdateCameraInput(float time, TypeOfActiveCamera type)
    {
        yield return new WaitForSeconds(time);

        TypeOfCamera = type;

        _isBlendingCamera = false;

        EndBlendAction.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        _inputRotation = new Vector2();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        switch (TypeOfCamera)
        {
            case TypeOfActiveCamera.Player:
                PlayerCameraRotation();
                break;

            case TypeOfActiveCamera.Binocucom:
                BinocucomRotation();
                break;

            default:
                break;
        }
    }

    void PlayerCameraRotation()
    {
        if (_cameraFollow != null && _inputRotation != Vector2.zero)
        {

            /*
            Vector3 rotation = new Vector3();

            rotation.x = _inputRotation.y* _mouseSensitivity;
            rotation.y = _inputRotation.x * _mouseSensitivity;
            rotation.z = 0;

            _cameraFollow.Rotate(rotation);

            var angle = _cameraFollow.eulerAngles;

            angle.z = 0;

            _cameraFollow.eulerAngles = angle;
            */


            Quaternion nextRotation = _cameraFollow.transform.rotation;
            nextRotation *= Quaternion.AngleAxis(-_inputRotation.y * _mouseSensitivity, Vector3.right);
            nextRotation *= Quaternion.AngleAxis(_inputRotation.x * _mouseSensitivity, Vector3.up);

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

    void BinocucomRotation()
    {
        if (_body != null && _binocucomCamera != null && _inputRotation != Vector2.zero)
        {

            //Rotate Player towards direction
            Vector3 rotation = Vector3.zero;
            rotation.y = _inputRotation.x * _mouseSensitivity;
            _body.Rotate(rotation);

            //Rotate Camera on Vertical Axis
            rotation = Vector3.zero;
            rotation.x = -_inputRotation.y * _mouseSensitivity;
            _binocucomCamera.transform.Rotate(rotation);

            //Clamp Camera on Vertical Axis
            rotation = _binocucomCamera.transform.localEulerAngles;
            rotation.x = Mathf.Clamp(rotation.x, 30, 150);
            _binocucomCamera.transform.localEulerAngles = rotation;

        }
    }
}
