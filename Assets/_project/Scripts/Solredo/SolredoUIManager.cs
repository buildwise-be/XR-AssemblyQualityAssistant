using _project.Scripts;
using MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.Events;

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

    void Start()
    {
        _dialogPool = GetComponent<DialogPool>();
        _placementManager = FindObjectOfType<PlacementManager>();
        _qualityManager = FindObjectOfType<QualityManager>();
        ShowInfoDialog("D�marrage", "Pour commencer, 'clickez' des doigts pour placer le mod�le r�duit de la maison.", OnIntroductionDone);
    }

    private void ShowIntroDialog()
    {
        d = (Dialog)_dialogPool.Get()
       .SetHeader("Instructions")
       .SetBody("Une fois dans l'application, placez le mod�le de dalle b�ton en 'cliquant' des doigts.\r\n" +
       "D�placez-le ensuite de mani�re � ce qu'il soit superpos� � sa contrepartie r�elle.\r\n" +
       "\r\nUne fois plac�, utilisez le menu main pour commencer le contr�le qualit�.")
       .SetPositive("Ok", (args) => { OnIntroductionDone?.Invoke(); d.Dismiss(); })
       .Show();
    }

    public void ShowInfoDialog(string header, string body, UnityEvent action)
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
       .SetHeader("Contr�le Qualit�")
       .SetBody("Effectuez chaque v�rification une � une.")
       .SetPositive("Commencer", (args) => 
       {
           _qualityManager.UnHighlightCurrentInspectedItem();
           QualityItem q = _qualityManager.GetNextInspectionElement();
           UpdateQualityDialog(q);
           _qualityManager.HighlightCurrentInspectedItem(); 
       })
       .SetNegative("Annuler", (args) => { d.Dismiss(); _qualityManager.ResetQualityControl(); })
       .Show();
    }

    private void UpdateQualityDialog(QualityItem q)
    {
        d = (Dialog)_dialogPool.Get()
       .SetHeader("Contr�le Qualit�")
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
}
