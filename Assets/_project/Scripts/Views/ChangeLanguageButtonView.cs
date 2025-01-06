using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ChangeLanguageButtonView : MonoBehaviour
{
    private int _localeIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        
        if (PlayerPrefs.HasKey(LanguageKey))
        {
            var currentLanguage = PlayerPrefs.GetString(LanguageKey);
            for(var i =0;i<LocalizationSettings.AvailableLocales.Locales.Count;i++)
            {
                if (LocalizationSettings.AvailableLocales.Locales[i].LocaleName == currentLanguage)
                {
                    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
                    _localeIndex = i;
                }
            }
            
        }
        Debug.Log(_localeIndex+ " - "+LocalizationSettings.SelectedLocale.Identifier);
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.L)) OnNextLocaleShowed();
        #endif
    }
    
    public void OnNextLocaleShowed()
    {
        _localeIndex = _localeIndex + 1 >= LocalizationSettings.AvailableLocales.Locales.Count ? 0 : _localeIndex + 1;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeIndex];
        //Debug.Log(_localeIndex+ " - "+LocalizationSettings.SelectedLocale.Identifier);
        PlayerPrefs.SetString(LanguageKey,LocalizationSettings.SelectedLocale.LocaleName);
    }

    private const string LanguageKey = "LANGUAGE-ID";
}
