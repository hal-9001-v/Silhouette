using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountEvent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int _targetCount;
    [SerializeField] bool _onlyOnce = true;

    [SerializeField] UnityEvent _event;

    int _currentCount;
    bool _done;

    /// <summary>
    /// Add one to the counter
    /// </summary>
    public void AddCount()
    {
        AddCount(1);
    }

    /// <summary>
    /// Add n to the counter
    /// </summary>
    /// <param name="n"></param>
    public void AddCount(int n)
    {
        if (_done && _onlyOnce) return;

        _targetCount += n;

        if (_targetCount <= _currentCount)
        {
            _done = true;

            _event.Invoke();

        }

    }


}
