using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CleanerBehaviour : PlayableBehaviour
{
    bool _done;
 
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (_done == false)
        {
            var generator = MonoBehaviour.FindObjectOfType<DialogueGenerator>();

            if (generator) {
                generator.Clear();

                _done = true;
            }
     
        }
    }
}

