using System;
using _project.Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

internal class DisplayOverride : MonoBehaviour
{
    [SerializeField] TMP_Text _displayText;
    public void OverrideDisplayProcess(int i, AssemblyStep step)
    {
        var currentLanguage = LocalizationSettings.SelectedLocale.LocaleName;
        _displayText.SetText(step.Indications[0].GetText(currentLanguage));
    }
}