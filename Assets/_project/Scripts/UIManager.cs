using System;
using MixedReality.Toolkit.UX;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UIManager : MonoBehaviour
{
    private DialogPool _dialogPool;
    private PlacementManager _placementManager;
    private QualityManager _qualityManager;
    private Dialog d;
    
    private enum DialogType{
        IntroQR,
        IntroManual,
        IntroQualityDialog,

        EndQualityDialog,
        FinalizeQualityDialog,
        ChoosePlacementMethod
    }
    private DialogType _currentDialogType = DialogType.IntroManual;
    void Start()
    {
        _dialogPool = GetComponent<DialogPool>();
        _placementManager = FindObjectOfType<PlacementManager>();
        _qualityManager = FindObjectOfType<QualityManager>();

        LocalizationSettings.SelectedLocaleChanged += OnUpdateLocale;

        UpdateLocalizedText();


    }

    private void UpdateLocalizedText()
    {
        _showManualIntroDialogHeader = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable", "ShowManualIntroDialogHeader");
        _showManualIntroDialogBody = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable", "ShowManualIntroDialogBody");
        _showQRIntroDialogHeader = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable","ShowQRIntroDialogHeader");
        _showQRIntroDialogBody = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable","ShowQRIntroDialogBody");
        _choosePlacementMethodDialogBody = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable","ChoosePlacementMethodDialogBody");
        _choosePlacementMethodDialogHeader = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable","ChoosePlacementMethodDialogBody");
        _qualityControlHeader = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable","QualityControlDialogHeader");
        _qualityControlBody = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable","QualityControlDialogBody");
        _qualityControlValidate = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable","QualityControlDialogValidate");
        _qualityControlCancel = LocalizationSettings.StringDatabase.GetLocalizedString("InfoDialogueTable","QualityControlDialogCancel");
    }

    private void OnUpdateLocale(Locale obj)
    {
        UpdateLocalizedText();
        UpdateLocalizedText();

        if (d == null) return;
        if (!d.VisibleRoot==enabled) return;
        
        switch (_currentDialogType)
        {
            case DialogType.IntroQR:
                RefreshDialog(_showQRIntroDialogHeader, _showQRIntroDialogBody);
                break;
            case DialogType.IntroManual:
                RefreshDialog(_showManualIntroDialogHeader, _showManualIntroDialogBody);
                    
                break;
            case DialogType.IntroQualityDialog:
                
                break;
            case DialogType.EndQualityDialog:
                break;
            case DialogType.FinalizeQualityDialog:
                break;
            case DialogType.ChoosePlacementMethod:
                RefreshDialog(_choosePlacementMethodDialogHeader, _choosePlacementMethodDialogBody);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RefreshDialog(string showManualIntroDialogHeader, string s)
    {
        d.SetHeader(showManualIntroDialogHeader);
        d.SetBody(s);
        d.Show();
    }
    
    
    public string _showManualIntroDialogBody;
    private string _showManualIntroDialogHeader;
    public string _showQRIntroDialogBody;
    private string _showQRIntroDialogHeader;
    private string _choosePlacementMethodDialogBody;
    private string _choosePlacementMethodDialogHeader;
    private string _qualityControlHeader;
    private string _qualityControlBody;
    private string _qualityControlValidate;
    private string _qualityControlCancel;

    public void ShowManualIntroDialog()
    {
        _currentDialogType = DialogType.IntroManual;
        d = (Dialog) _dialogPool.Get()
            .SetHeader(_showManualIntroDialogHeader)//"Instructions")
            .SetBody(_showManualIntroDialogBody)
       .SetPositive("Ok", (args) => { 
                EnableSlabPlacement(); 
                d.Dismiss();
                d = null;
            })
       .Show();
    }
    
    public void ShowQRIntroDialog()
    {
        _currentDialogType = DialogType.IntroQR;
        d = (Dialog)_dialogPool.Get()
            .SetHeader(_showQRIntroDialogHeader)
            .SetBody(_showQRIntroDialogBody)/*"Une fois dans l'application, placez le QR au sol.\r\n" +
                                           "Scannez afin de placee automatiquement le modèle.\r\n" +
                                           "\r\nUne fois plac�, utilisez le menu main pour commencer le contr�le qualit�.")*/
            .SetPositive("Ok", (args) => { EnableSlabPlacement(); d.Dismiss();
                d = null;
            })
            .Show();
    }

    public void ShowInfoDialog(string header, string body)
    {
        d = (Dialog)_dialogPool.Get()
       .SetHeader(header)
       .SetBody(body)
       .SetPositive("Ok", (args) => { d.Dismiss(); })
       .Show();
    }

    public void ShowQualityDialog()
    {
        _placementManager.LockSlabPosition();
        d = (Dialog)_dialogPool.Get()
       .SetHeader(_qualityControlHeader/*"Contr�le Qualit�"*/)
       .SetBody(_qualityControlBody/*"Effectuez chaque v�rification une � une."*/)
       .SetPositive(_qualityControlValidate/*"Commencer"*/, (args) => 
       {
           _qualityManager.UnHighlightCurrentInspectedItem();
           QualityItem q = _qualityManager.GetNextInspectionElement();
           //UpdateQualityDialog(q);
           _qualityManager.HighlightCurrentInspectedItem(); 
       })
       .SetNegative(_qualityControlCancel/*"Annuler"*/, (args) => { d.Dismiss(); _qualityManager.ResetQualityControl(); })
       .Show();
    }

    private void UpdateQualityDialog(QualityItem q)
    {
        d = (Dialog)_dialogPool.Get()
       .SetHeader(_qualityControlHeader)
       .SetBody(q.Instructions)
       .SetPositive("Suivant", (args) =>
       {
           _qualityManager.UnHighlightCurrentInspectedItem();
           QualityItem q = _qualityManager.GetNextInspectionElement();
           if (q == null)
           {
               d.Dismiss();
               _qualityManager.ResetQualityControl();
               ShowEndQualityDialog();
               return;
           }
           //UpdateQualityDialog(q);
           _qualityManager.HighlightCurrentInspectedItem();
       })
       .SetNegative("Arr�ter", (args) => { d.Dismiss(); _qualityManager.ResetQualityControl(); })
       .Show();
    }

    private void ShowEndQualityDialog()
    {
        _currentDialogType = DialogType.EndQualityDialog;
        d = (Dialog)_dialogPool.Get()
       .SetHeader("Contr�le Qualit�")
       .SetBody("Tous les points d'attention ont �t� contr�l�s!")
       .SetPositive("Terminer", (args) =>
       {
           d.Dismiss();
       })
       .Show();
    }

    public void ShowFinalizeDialog()
    {
        _currentDialogType = DialogType.FinalizeQualityDialog;
        d = (Dialog)_dialogPool.Get()
      .SetHeader("Valider pour production")
      .SetBody("Etes-vous certain de vouloir valider le contr�le qualit� et envoyer le signal pour production ?")
      .SetPositive("Oui", (args) =>
      {
          d.Dismiss();
      })
      .SetNegative("Non", (args) => { d.Dismiss(); })
      .Show();
    }

    void EnableSlabPlacement()
    {
        StartCoroutine(EnableSlabPlacementCoroutine());
    }

    IEnumerator EnableSlabPlacementCoroutine()
    {
        yield return new WaitForSeconds(1);
        _placementManager.IsSlabCreationAuthorized = true;
    }

    public async Task<int>  DisplayStartProcessMessage()
    {
        _currentDialogType = DialogType.ChoosePlacementMethod;
        var decision = 0;
        d = (Dialog)_dialogPool.Get()
            .SetHeader(_choosePlacementMethodDialogHeader)
            .SetBody(_choosePlacementMethodDialogBody)
            .SetPositive("QR Code", (args) =>
            {
                decision = 1;
                d.Dismiss();
            })
            .SetNegative("Manual", (args) =>
            {
                decision = 2;
                d.Dismiss();
            })
            .Show();
        while (decision == 0)
        {
            await Task.Yield();
        }
        return decision;
        
    }
}
