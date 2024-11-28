using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Indication
{
    public int IndicationID;
    [HideInInspector] public string IndicationText;
    [FormerlySerializedAs("LocalizedIndications")] public LocalizedStringData[] Text;
    public string GetText(string _language)
    {
        //LocalizationSettings
        foreach (var VARIABLE in Text)
        {
            if (_language == VARIABLE.Language) return VARIABLE.TextData;
        }

        return "NO data for this Language";
    }
}