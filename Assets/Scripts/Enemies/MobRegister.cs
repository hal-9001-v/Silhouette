using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRegister : MonoBehaviour
{
    public List<Mob> mobs;
    public List<Listener> listeners;
    public List<Pocket> pockets;

    private void Awake()
    {
        mobs = new List<Mob>();
        listeners = new List<Listener>();
        pockets = new List<Pocket>();
    }



}
