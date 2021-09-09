using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueBehaviour : PlayableBehaviour
{
    public string text;
    public float typeDelay;
    public BoxLocation location;
    public BoxAppereance appereance;

    double _elapsedTime;

    bool _ended = false;
    bool _spawned = false;

    DialogueGenerator _dialogueGenerator;

    const float ElapsedTimeMultiplier = 1.1f;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (_ended == false)
        {
            if (_spawned == false)
            {
                _dialogueGenerator = MonoBehaviour.FindObjectOfType<DialogueGenerator>();

                if (_dialogueGenerator)
                {

                    _spawned = _dialogueGenerator.SpawnDialogue(this);
                }

            }

            _elapsedTime += info.deltaTime;

            if (_elapsedTime * ElapsedTimeMultiplier >= playable.GetDuration())
            {
                if (_dialogueGenerator)
                    _dialogueGenerator.EndOfClip();

                _ended = true;
            }
        }
    }
}