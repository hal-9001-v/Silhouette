using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Cutscene : InputComponent
{
    PlayableDirector _director;
    PlatformMap _input;

    [Header("Settings")]
    [SerializeField] bool _playOnStart;
    [SerializeField] bool _disableInput = true;


    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();

        _director.playOnAwake = false;

        if (_disableInput)
            _director.stopped += EnableInput;



        _director.stopped += HideDialogue;

    }

    void HideDialogue(PlayableDirector director)
    {
        var dialogueGenerator = FindObjectOfType<DialogueGenerator>();

        if (dialogueGenerator)
            dialogueGenerator.Clear();
    }

    void EnableInput(PlayableDirector director)
    {
        _input.Character.Enable();
    }

    void DisableInput()
    {
        _input.Character.Disable();
    }

    private void Start()
    {
        if (_playOnStart)
        {
            TriggerCutscene();
        }
    }

    public void TriggerCutscene()
    {
        var generator = FindObjectOfType<DialogueGenerator>();

        if (generator)
        {
            generator.currentDirector = _director;
            _director.Play();

            if (_disableInput)
                DisableInput();
        }
    }

    public override void SetInput(PlatformMap input)
    {
        _input = input;

    }
}
