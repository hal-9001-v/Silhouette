using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTarget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RawImage _targetAim;

    [Header("Settings")]
    [SerializeField] LayerMask _blockingMask;


    Vector2 _screenCenter
    {
        get
        {
            return new Vector2(Screen.width, Screen.height) * 0.5f;
        }
    }

    Camera _camera;



    private void Awake()
    {

        var brain = FindObjectOfType<CinemachineBrain>();

        if (brain)
        {

            _camera = brain.GetComponent<Camera>();
        }
    }

    public bool IsObjectInAim(Transform target)
    {
        if (!_targetAim) return false;

        Vector3 rawPositionWorld = _camera.WorldToScreenPoint(target.position);

        if (!(rawPositionWorld.x > _targetAim.rectTransform.position.x - Mathf.Abs(_targetAim.rectTransform.sizeDelta.x) * 0.5f)) return false;

        if (!(rawPositionWorld.x < _targetAim.rectTransform.position.x + Mathf.Abs(_targetAim.rectTransform.sizeDelta.x) * 0.5f)) return false;

        if (!(rawPositionWorld.y > _targetAim.rectTransform.position.y - Mathf.Abs(_targetAim.rectTransform.sizeDelta.y) * 0.5f)) return false;


        if (!(rawPositionWorld.y < _targetAim.rectTransform.position.y + Mathf.Abs(_targetAim.rectTransform.sizeDelta.y) * 0.5f)) return false;

        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        if (Physics.Raycast(transform.position, direction, Vector3.Distance(target.position, transform.position), _blockingMask)) return false;

        return true;
    }




}
