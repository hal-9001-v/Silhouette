using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueGenerator : InputComponent
{
    [Header("References")]
    [SerializeField] Transform _leftPosition;
    [SerializeField] Transform _rightPosition;
    [SerializeField] Transform _centerDownPosition;

    [Header("Prefabs")]
    [SerializeField] DialogueBox _silBoxPrefab;
    [SerializeField] DialogueBox _hackerBoxPrefab;

    List<DialogueBox> _displayedBoxes;
    DialogueBox _currentDialogueBox;

    //_started is true when Start has executed. This is to make sure that this script is not used on editor by any timeline due to this script instanciates objects.
    bool _started;

    public PlayableDirector currentDirector { get; set; }

    double _timelineSpeed
    {
        get
        {
            if (currentDirector)
                return currentDirector.playableGraph.GetRootPlayable(0).GetSpeed();
            else
                return -1;
        }

        set
        {
            if (currentDirector)
            {
                if (currentDirector.playableGraph.IsValid())
                    currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(value);
            }
        }
    }


    private void Awake()
    {
        _displayedBoxes = new List<DialogueBox>();
    }

    private void Start()
    {
        _started = true;
    }

    public bool SpawnDialogue(DialogueBehaviour dialogueBehaviour)
    {
        //Dont let this function be called on Editor mode so it doesnt spawn permanent textboxes.
        if (!_started)
            return false;

        DialogueBox boxPrefab;
        switch (dialogueBehaviour.appereance)
        {
            case BoxAppereance.Sil:
                boxPrefab = _silBoxPrefab;
                break;

            case BoxAppereance.Hacker:
                boxPrefab = _hackerBoxPrefab;
                break;


            default:
                boxPrefab = null;
                break;
        }

        if (boxPrefab)
        {
            var dialogueBox = Instantiate(boxPrefab);
            dialogueBox.transform.parent = transform;
            Vector3 position;
            switch (dialogueBehaviour.location)
            {
                case BoxLocation.Left:
                    position = _leftPosition.position;
                    break;

                case BoxLocation.Right:
                    position = _rightPosition.position;
                    break;
                case BoxLocation.CenterDown:
                    position = _centerDownPosition.position;
                    break;

                default:
                    position = Vector3.zero;

                    break;
            }

            position -= (new Vector3(0, 130, 0)) * _displayedBoxes.Count;

            dialogueBox.StartDialogue(position, dialogueBehaviour.text, dialogueBehaviour.typeDelay);

            _displayedBoxes.Add(dialogueBox);

            _currentDialogueBox = dialogueBox;
            return true;
        }
        return false;
    }

    public void Clear()
    {
        if (_displayedBoxes != null)
        {
            foreach (DialogueBox box in _displayedBoxes)
            {
                box.Clear();
            }

            _displayedBoxes.Clear();

            _currentDialogueBox = null;
        }

    }

    public void EndOfClip()
    {
        _timelineSpeed = 0;
    }

    void ResumeTimeline()
    {
        _timelineSpeed = 1;
    }

    public override void SetInput(PlatformMap input)
    {
        input.Cutscene.Interact.performed += ctx =>
        {
            if (_currentDialogueBox)
            {
                if (_currentDialogueBox.isTextComplete)
                {
                    ResumeTimeline();
                }
                else
                {
                    _currentDialogueBox.Complete();
                }
            }

        };


    }


}
