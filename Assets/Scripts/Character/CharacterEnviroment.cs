using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEnviroment : MonoBehaviour
{
    HeavyMob[] _mobs;

    private void Awake()
    {
        _mobs = FindObjectsOfType<HeavyMob>();

    }

    private void FixedUpdate()
    {
        
    }


}
