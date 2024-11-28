using System;
using UnityEngine;

[Serializable]
public class Indication
{
    public int IndicationID;
    [HideInInspector] public string IndicationText;
    public LocalizedStringData[] LocalizedIndications;
    public string GetText(string _language)
    {
        //LocalizationSettings
        foreach (var VARIABLE in LocalizedIndications)
        {
            if (_language == VARIABLE.Language) return VARIABLE.TextData;
        }

        return "NO data for this Language";
    }
}