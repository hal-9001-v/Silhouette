using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform[] _entries;
    [SerializeField] Collider[] _exitColliders;
    [SerializeField] CharacterVent _characterVent;
    // Start is called before the first frame update
    void Awake()
    {
        _characterVent = FindObjectOfType<CharacterVent>();

        foreach (Collider collider in _exitColliders)
        {
            var del = collider.gameObject.AddComponent<ColliderDelegate>();

            del.TriggerEnterAction += ExitCharacter;
        }

    }

    public void EnterVent(int index)
    {
        if (_entries != null && _entries.Length != 0)
        {
            if (index >= 0 && index < _entries.Length && _entries[index] != null)
            {
                _characterVent.EnterVent(_entries[index].position);
            }
        }
    }



    void ExitCharacter(Collider coll, Vector3 pos)
    {
        var character = coll.GetComponent<CharacterVent>();

        if (character != null)
        {
            if (character.isOnVent)
            {
                character.ExitVent();
            }

        }

    }

}
