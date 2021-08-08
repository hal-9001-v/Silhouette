using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaphore
{
    int _lockCount;

    public bool isOpen
    {
        get {
            return _lockCount == 0;
        }
    }

    public Semaphore() {
        _lockCount = 0;
    }

    public void Lock()
    {
        _lockCount++;
    }

    public void Unlock()
    {
        if (_lockCount > 0) _lockCount--;

    }
}
