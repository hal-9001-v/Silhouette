using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public Action<Dictionary<string, BasicData>> onSaveAction;

    public Dictionary<string, BasicData> dataDictionary;

    const string FileName = "/saveData.data";

    string DataPath
    {
        get
        {
            return Application.persistentDataPath + FileName;
        }
    }

    private void Awake()
    {
        LoadDictionary();
    }



    [ContextMenu("Save Dictionary")]
    void SaveDictionary()
    {
        if (onSaveAction != null)
        {
            //Reset and Update Dictionary
            dataDictionary = new Dictionary<string, BasicData>();

            onSaveAction.Invoke(dataDictionary);

            //Save Data
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(DataPath, FileMode.Create);

            formatter.Serialize(stream, dataDictionary);

            stream.Close();
        }
    }

    void LoadDictionary()
    {
        if (File.Exists(DataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(DataPath, FileMode.Open);

            dataDictionary = formatter.Deserialize(stream) as Dictionary<string, BasicData>;

            stream.Close();
        }
        else
        {
            dataDictionary = new Dictionary<string, BasicData>();

            Debug.LogWarning("Couldn't find data file!");
        }
    }

}
