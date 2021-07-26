using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraZone : MonoBehaviour
{


    [Header("Settings")]
    [SerializeField] CinemachineVirtualCamera _camera;
    [Space(5)]
    [SerializeField] UnityEvent _enter;
    [SerializeField] UnityEvent _exit;
    
    int _colliderCounter;
    bool _dirt;
    bool _cameraInZone;

    PlayerCamera _playerCamera;

    private void Awake()
    {
        if (_camera != null)
        {
            _camera.enabled = false;
        }

        _playerCamera = FindObjectOfType<PlayerCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Colliders must overlap between them if zone is continuous
        foreach (CameraZoneCollider collider in GetComponentsInChildren<CameraZoneCollider>())
        {
            collider.EnterActions += IncreaseColliderCounter;
            collider.ExitActions += DecreaseColliderCounter;
        }
    }

    private void LateUpdate()
    {
        if (_dirt)
        {
            if (_cameraInZone)
            {
                ChangeToZoneCamera();
            }
            else
            {
                ResetCamera();
            }

            _dirt = false;
        }
    }

    void IncreaseColliderCounter()
    {
        //If Player is in now
        if (_colliderCounter == 0)
        {
            //Late Update has to execute completely because of dirt bit
            _dirt = true;
            _cameraInZone = true;
        }
        _colliderCounter++;
    }

    void DecreaseColliderCounter()
    {
        _colliderCounter--;

        //If Player is out now
        if (_colliderCounter == 0)
        {
            //Late Update has to execute completely because of dirt bit
            _dirt = true;
            _cameraInZone = false;
        }
    }

    void ChangeToZoneCamera()
    {

        if (_camera != null && _playerCamera != null)
        {
            _playerCamera.SetActiveCamera(_camera, PlayerCamera.TypeOfActiveCamera.Fixed);
        }

        _enter.Invoke();

    }

    void ResetCamera()
    {
        if (_camera != null && _playerCamera != null)
        {
            _playerCamera.ResetCamera();
        }

        _exit.Invoke();
    }

    [ContextMenu("Add CameraZoneCollider Scripts to Children")]
    void AddZoneCollidersToChildren()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.gameObject.AddComponent<CameraZoneCollider>();

            Debug.Log("CameraZoneCollider Added to " + collider.gameObject.name);
        }
    }

}
