using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDistributor : MonoBehaviour
{

    PlatformMap _inputMap;
    static InputDistributor _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            _inputMap = new PlatformMap();
            

            SetInput();
            _inputMap.Enable();

            PlayerCamera pc = FindObjectOfType<PlayerCamera>();
        }

    }

    private void OnDisable()
    {
        if (_inputMap != null)
            _inputMap.Disable();
    }

    private void OnEnable()
    {
        if (_inputMap != null)
            _inputMap.Enable();
    }

    void SetInput()
    {

        if (_inputMap != null)
        {
            foreach (InputComponent components in FindObjectsOfType<InputComponent>())
            {
                components.SetInput(_inputMap);
            }
        }


    }
}
