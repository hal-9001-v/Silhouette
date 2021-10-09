using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public void LoadData();

    public void SaveData(Dictionary<string, BasicData> dictionary);

    public void AddSaveCallback();

}
