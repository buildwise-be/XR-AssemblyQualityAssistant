using MixedReality.Toolkit.UX;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private DialogPool _dialogPool;
    private PlacementManager _placementManager;
    private QualityManager _qualityManager;
    private Dialog d;
    void Start()
    {
        _dialogPool = GetComponent<DialogPool>();
        _placementManager = FindObjectOfType<PlacementManager>();
        _qualityManager = FindObjectOfType<QualityManager>();
    }

    public void ShowManualIntroDialog()
    {
        d = (Dialog)_dialogPool.Get()
       .SetHeader("Instructions")
       .SetBody("Une fois dans l'application, placez le mod�le de dalle b�ton en 'cliquant' des doigts.\r\n" +
       "D�placez-le ensuite de mani�re � ce qu'il soit superpos� � sa contrepartie r�elle.\r\n" +
       "\r\nUne fois plac�, utilisez le menu main pour commencer le contr�le qualit�.")
       .SetPositive("Ok", (args) => { EnableSlabPlacement(); d.Dismiss(); })
       .Show();
    }
    
    public void ShowQRIntroDialog()
    {
        d = (Dialog)_dialogPool.Get()
            .SetHeader("Instructions")
            .SetBody("Une fois dans l'application, placez le QR au sol.\r\n" +
                     "Scannez afin de placee automatiquement le modèle.\r\n" +
                     "\r\nUne fois plac�, utilisez le menu main pour commencer le contr�le qualit�.")
            .SetPositive("Ok", (args) => { EnableSlabPlacement(); d.Dismiss(); })
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
        StartCoroutine(EnableSlabPlacementCoroutine());
    }

    IEnumerator EnableSlabPlacementCoroutine()
    {
        yield return new WaitForSeconds(1);
        _placementManager.IsSlabCreationAuthorized = true;
    }

    public async Task<int>  DisplayStartProcessMessage()
    {
        var decision = 0;
        d = (Dialog)_dialogPool.Get()
            .SetHeader("Choose your placement method")
            .SetBody("Which placement method would you like to use?")
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
