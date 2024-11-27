using System;
using _project.Scripts;
using MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class SolredoUIManager : MonoBehaviour
{
    private DialogPool _dialogPool;
    private PlacementManager _placementManager;
    private QualityManager _qualityManager;
    private Dialog d;

    //[HideInInspector]
    public UnityEvent OnIntroductionDone;
    //[HideInInspector]
    public UnityEvent OnStartFindingPlane;

    //[SerializeField] private DashboardManager _dashboardManager;
    //[SerializeField] private GameObject _dashboard;
    [SerializeField] QualityAssistantSceneBootstrap _assemblyBootStrap;
    private LocalizedString _introHeader = new LocalizedString("InfoDialogueTable", "introHeader");
    private LocalizedString _introMessage = new LocalizedString("InfoDialogueTable", "introMessage");
    private LocalizedString _qualityControlMessage = new LocalizedString("InfoDialogueTable", "qualityControlMessage");
    private LocalizedString _qualityControlHeader = new LocalizedString("InfoDialogueTable", "qualityControlHeader");
    private LocalizedString _qualityControlStart = new LocalizedString("InfoDialogueTable", "qualityControlStart");
    private LocalizedString _qualityControlCancel = new LocalizedString("InfoDialogueTable", "qualityControlCancel");
    private LocalizedString _qualityControlNext = new LocalizedString("InfoDialogueTable", "qualityControlNext");
    private LocalizedString _QRScanHeader = new LocalizedString("InfoDialogueTable", "QRScanHeader");
    private LocalizedString _QRScanMessage = new LocalizedString("InfoDialogueTable", "QRScanMessage");


    private string _introHeaderValue => _introHeader.GetLocalizedString();
    private string _introMessageValue => _introMessage.GetLocalizedString();
    private string _qualityControlMessageValue => _qualityControlMessage.GetLocalizedString();
    private string _qualityControlHeaderValue => _qualityControlHeader.GetLocalizedString();
    private string _qualityControlStartValue => _qualityControlStart.GetLocalizedString();
    private string _qualityControlCancelValue => _qualityControlCancel.GetLocalizedString();
    private string _qualityControlNextValue => _qualityControlNext.GetLocalizedString();
    private string _QRScanMessageValue => _QRScanMessage.GetLocalizedString();
    private string _QRScanHeaderValue => _QRScanHeader.GetLocalizedString();
    void Start()
    {

        
        _placementManager = FindObjectOfType<PlacementManager>();
        _qualityManager = FindObjectOfType<QualityManager>();
        
    }

    private void Awake()
    {
        _dialogPool = GetComponent<DialogPool>();
    }

    public void ShowStartHousePhaseDialog()
    {
        ShowInfoDialog(_introHeaderValue, _introMessageValue/*""*/, OnIntroductionDone);
    }

    private void ShowIntroDialog()
    {
        d = (Dialog)_dialogPool.Get()
            .SetHeader(_introHeaderValue/*"Instructions"*/)
            .SetBody(_introMessageValue/*"Une fois dans l'application, placez le modèle de dalle béton en 'cliquant' des doigts.\r\n" +
                                  "Déplacez-le ensuite de manière à ce qu'il soit superpos� � sa contrepartie r�elle.\r\n" +
                                  "\r\nUne fois plac�, utilisez le menu main pour commencer le contr�le qualit�.")*/)
            .SetPositive("Ok", (args) => { OnIntroductionDone?.Invoke(); d.Dismiss(); })
            .Show();
    }

    private void ShowInfoDialog(string header, string body, UnityEvent action)
    {
        d = (Dialog)_dialogPool.Get()
       .SetHeader(header)
       .SetBody(body)
       .SetPositive("Ok", (args) => { action?.Invoke(); d.Dismiss(); })
       .Show();
    }

    public void ShowQualityDialog()
    {
        _placementManager.LockSlabPosition();
        d = (Dialog)_dialogPool.Get()
       .SetHeader(_qualityControlHeaderValue/**/)
       .SetBody(_qualityControlMessageValue/*"Effectuez chaque v�rification une � une."*/)
       .SetPositive(_qualityControlStartValue/*"Commencer"*/, (args) => 
       {
           _qualityManager.UnHighlightCurrentInspectedItem();
           QualityItem q = _qualityManager.GetNextInspectionElement();
           UpdateQualityDialog(q);
           _qualityManager.HighlightCurrentInspectedItem(); 
       })
       .SetNegative(_qualityControlCancelValue, (args) => { d.Dismiss(); _qualityManager.ResetQualityControl(); })
       .Show();
    }

    private void UpdateQualityDialog(QualityItem q)
    {
        d = (Dialog)_dialogPool.Get()
       .SetHeader(_qualityControlHeaderValue)
       .SetBody(q.Instructions)
       .SetPositive(_qualityControlNextValue/*"Suivant"*/, (args) =>
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
           UpdateQualityDialog(q);
           _qualityManager.HighlightCurrentInspectedItem();
       })
       .SetNegative("Arr�ter", (args) => { d.Dismiss(); _qualityManager.ResetQualityControl(); })
       .Show();
    }



    private void ShowEndQualityDialog()
    {
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
        _placementManager.IsSlabCreationAuthorized = true;
    }

    public void ShowStepDashboard()
    {
        _assemblyBootStrap.StartAssemblyProcess();
    }

    public void ShowQRScanInfoDialog()
    {
        ShowInfoDialog(_QRScanHeaderValue, _QRScanMessageValue/*"Scannez le QR Code pour placer le module choisi"*/, null);
    }
}
