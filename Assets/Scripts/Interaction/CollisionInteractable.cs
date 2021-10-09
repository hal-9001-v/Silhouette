using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CollisionInteractable : MonoBehaviour, ISaveable
{
    [Header("Settings")]

    [SerializeField] bool _enterOnlyOnce;
    [SerializeField] bool _exitOnlyOnce;

    [SerializeField] UnityEvent _enterEvent;
    [SerializeField] UnityEvent _exitEvent;

    [Header("Save")]
    [SerializeField] bool _save;

    bool _enterDone;
    bool _exitDone;

    private void Start()
    {
        if (_save)
        {
            AddSaveCallback();

            LoadData();
        }
    }

    public void EnterInteraction()
    {
        if (_enterOnlyOnce && _enterDone)
            return;

        _enterDone = true;

        _enterEvent.Invoke();

    }

    public void ExitInteraction()
    {
        if (_exitOnlyOnce && _exitDone)
            return;

        _exitDone = true;

        _exitEvent.Invoke();

    }

    public void SaveData(Dictionary<string, BasicData> dictionary)
    {
        BasicData data = new BasicData();

        data.done = new bool[2];

        data.done[0] = _enterDone;
        data.done[1] = _exitDone;

        dictionary.Add(name, data);


    }

    public void LoadData()
    {
        var storage = FindObjectOfType<DataStorage>();

        if (storage)
        {
            BasicData data;

            if (storage.dataDictionary.TryGetValue(name, out data))
            {
                _enterDone = data.done[0];
                _exitDone = data.done[1];
            }

        }

    }

    public void AddSaveCallback()
    {
        var storage = FindObjectOfType<DataStorage>();

        if (storage)
        {
            storage.onSaveAction += SaveData;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractionTrigger>() != null)
        {
            EnterInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractionTrigger>() != null)
        {
            ExitInteraction();
        }
    }


}
