using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRegister : MonoBehaviour
{
    public List<HeavyMob> mobs;

    private void Awake()
    {
        mobs = new List<HeavyMob>();
    }

}
