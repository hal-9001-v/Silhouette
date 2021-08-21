using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Rumbler : MonoBehaviour
{
    float _duration;
    Timer _timer;

    float _highSpeed;
    float _lowSpeed;

    public bool isRumbling { get; private set; }

    public Gamepad gamepad
    {
        get
        {
            if (Gamepad.all.Count != 0)
            {
                return Gamepad.all[0];
            }
            else
            {
                return null;
            }
        }
    }

    private void Awake()
    {
        _timer = new Timer();

    }

    private void Update()
    {
        if (isRumbling)
        {
            if (_timer.UpdateTimer(_duration))
            {
                isRumbling = false;
                SetSpeed(0,0);
                return;
            }

            SetSpeed(_lowSpeed, _highSpeed);
        }
    }

    /// <summary>
    /// Set motors velocity from 0 to 1.
    /// </summary>
    /// <param name="highSpeed"></param>
    /// <param name="lowSpeed"></param>
    /// <param name="duration"></param>
    public void Rumble(float lowSpeed, float highSpeed, float duration)
    {
        _timer.ResetTimer();

        isRumbling = true;

        _lowSpeed = lowSpeed;
        _highSpeed = highSpeed;

        _duration = duration;
    }

    public void StopRumble()
    {
        SetSpeed(0, 0);

        isRumbling = false;
    }

    bool SetSpeed(float lowSpeed, float highSpeed)
    {
        var pad = gamepad;

        if (pad != null)
        {
            pad.SetMotorSpeeds(lowSpeed, highSpeed);
            return true;
        }
        else
        {
            return false;
        }
    }
}