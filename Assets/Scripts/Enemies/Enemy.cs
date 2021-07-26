using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Coroutine CountDownToAction(float seconds, Action action)
    {
        return StartCoroutine(CountDown(seconds, action));
    }

    static private IEnumerator CountDown(float seconds, Action action)
    {

        yield return new WaitForSeconds(seconds);

        action.Invoke();

    }

}
