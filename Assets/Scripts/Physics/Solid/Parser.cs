using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser
{
    public static string[] ParseTextFile(TextAsset fileName){

        string[] textString = fileName.text.Split(new string[] { " ", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

        return textString;
    }
    
}
