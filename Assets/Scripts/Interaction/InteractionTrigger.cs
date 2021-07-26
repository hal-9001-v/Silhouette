using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : InputComponent
{
    InputInteractable[] _inputInteractables;
    //Observer
    public override void SetInput(PlatformMap input)
    {
        input.Character.Interact.performed += ctx =>
        {
            foreach (InputInteractable interactable in _inputInteractables)
            {
                interactable.PressInteraction();
            }
        };

        input.Character.Interact.canceled += ctx =>
        {
            foreach (InputInteractable interactable in _inputInteractables)
            {
                interactable.ReleaseInteraction();
            }
        };
    }

    private void Start()
    {
        _inputInteractables = FindObjectsOfType<InputInteractable>();
    }



}
