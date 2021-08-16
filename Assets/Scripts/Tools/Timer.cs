using UnityEngine;

public class Timer
{
    public float elapsedFixedTime { get; private set; }
    public float elapsedTime { get; private set; }

    public bool UpdateFixedTimer(float value)
    {
        if (value <= elapsedFixedTime)
        {
            return true;
        }
        else
        {
            elapsedFixedTime += Time.fixedDeltaTime;
            return false;
        }
    }

    public void ResetFixedTimer()
    {
        elapsedFixedTime = 0;
    }

    public bool UpdateTimer(float value)
    {
        if (value <= elapsedTime)
        {
            return true;
        }
        else
        {
            elapsedTime += Time.deltaTime;
            return false;
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0;
    }


}
