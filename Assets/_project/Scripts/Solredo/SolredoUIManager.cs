using MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.Events;

public class SolredoUIManager : MonoBehaviour
{
    private DialogPool _dialogPool;
    private PlacementManager _placementManager;
    private QualityManager _qualityManager;
    private Dialog d;

    [HideInInspector]
    public UnityEvent OnIntroductionDone;
    [HideInInspector]
    public UnityEvent OnStartFindingPlane;

    [SerializeField] private DashboardManager _dashboardManager;
    [SerializeField] private GameObject _dashboard;

    void Start()
    {
        _dialogPool = GetComponent<DialogPool>();
        _placementManager = FindObjectOfType<PlacementManager>();
        _qualityManager = FindObjectOfType<QualityManager>();
        ShowInfoDialog("Démarrage", "Pour commencer, 'clickez' des doigts pour placer le modèle réduit de la maison.", OnIntroductionDone);
    }

    private void ShowIntroDialog()
    {
        d = (Dialog)_dialogPool.Get()
       .SetHeader("Instructions")
       .SetBody("Une fois dans l'application, placez le modèle de dalle béton en 'cliquant' des doigts.\r\n" +
       "Déplacez-le ensuite de manière à ce qu'il soit superposé à sa contrepartie réelle.\r\n" +
       "\r\nUne fois placé, utilisez le menu main pour commencer le contrôle qualité.")
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
       .SetHeader("Contrôle Qualité")
       .SetBody("Effectuez chaque vérification une à une.")
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
       .SetHeader("Contrôle Qualité")
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
       .SetNegative("Arrêter", (args) => { d.Dismiss(); _qualityManager.ResetQualityControl(); })
       .Show();
    }

    private void ShowEndQualityDialog()
    {
        d = (Dialog)_dialogPool.Get()
       .SetHeader("Contrôle Qualité")
       .SetBody("Tous les points d'attention ont été contrôlés!")
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
      .SetBody("Etes-vous certain de vouloir valider le contrôle qualité et envoyer le signal pour production ?")
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
        _dashboard.SetActive(true);
        _dashboard.transform.SetPositionAndRotation(Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Camera.main.transform.rotation);
        _dashboardManager.UpdateDashboardWithCurrentStep();
    }

    public void HideStepDashboard()
    {
        _dashboard.SetActive(false);
    }
}
