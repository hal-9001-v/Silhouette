using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider[] _ventColliders;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Collider collider in _ventColliders)
        {
            var del = collider.gameObject.AddComponent<ColliderDelegate>();

            del.TriggerEnterAction += EnterVent;
            del.TriggerExitAction += ExitVent;
        }

    }

    public void EnterVent(Transform source, Collider coll, Vector3 pos)
    {
        var character = coll.GetComponent<CharacterVent>();

        if (character != null)
        {
            character.AddVentCounter();
        }
    }

    void ExitVent(Transform source, Collider coll, Vector3 pos)
    {
        var character = coll.GetComponent<CharacterVent>();

        if (character != null)
        {
            character.RemoveVentCounter();
        }

    }

    public void ForceExit() {
        var character = FindObjectOfType<CharacterVent>();

        if (character)
            character.ExitVent();
    }
}
