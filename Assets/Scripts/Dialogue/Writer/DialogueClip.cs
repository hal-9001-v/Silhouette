using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    [TextArea(1, 5)]
    public string text;
    [Range(0, 1)] public float typeDelay;
    public BoxLocation location;
    public BoxAppereance appereance;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);

        DialogueBehaviour dialogueBehaviour = playable.GetBehaviour();
        dialogueBehaviour.text = text;
        dialogueBehaviour.typeDelay = typeDelay;
        dialogueBehaviour.location = location;
        dialogueBehaviour.appereance= appereance;


        return playable;
    }
}
