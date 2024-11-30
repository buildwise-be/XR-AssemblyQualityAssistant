using _project.Scripts.Controllers;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is solely responsible for updating the dashboard with the current step information.
/// </summary>
public class DashboardManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _imageHeaderText;
    [SerializeField] private UnityEngine.UI.Image _stepImage;
    [SerializeField] private RectTransform _indication1;
    [SerializeField] private RectTransform _indication2;
    [SerializeField] private RectTransform _indication3;
    [SerializeField] private RectTransform _indication4;
    [SerializeField] private RectTransform _indication5;

    private StepsManager _stepsManager;

    private void Start()
    {
        _stepsManager = FindObjectOfType<StepsManager>();
    }

    /// <summary>
    /// Updates the dashboard with the current step information.
    /// </summary>
    public void UpdateDashboardWithCurrentStep()
    {
        int currentStepIndex = _stepsManager.CurrentStepIndex;
        AssemblyStep currentStep = _stepsManager.GetSteps()[currentStepIndex];
        SetHeaderText(currentStep);
        SetIndicationsNumber(currentStep);
        SetIndicationsText(currentStep);
        SetStepIllustration(currentStep);
    }

    private void SetHeaderText(AssemblyStep step)
    {
        string text = "Etape " + step.StepID;
        _imageHeaderText.text = text;
    }

    private void SetIndicationsNumber(AssemblyStep step)
    {
        int nbIndications = step.Indications.Count;
        _indication1.gameObject.SetActive(false);
        _indication2.gameObject.SetActive(false);
        _indication3.gameObject.SetActive(false);
        _indication4.gameObject.SetActive(false);
        _indication5.gameObject.SetActive(false);

        switch (nbIndications)
        {
            case 1:
                _indication1.gameObject.SetActive(true);
                break;
            case 2:
                _indication1.gameObject.SetActive(true);
                _indication2.gameObject.SetActive(true);
                break;
            case 3:
                _indication1.gameObject.SetActive(true);
                _indication2.gameObject.SetActive(true);
                _indication3.gameObject.SetActive(true);
                break;
            case 4:
                _indication1.gameObject.SetActive(true);
                _indication2.gameObject.SetActive(true);
                _indication3.gameObject.SetActive(true);
                _indication4.gameObject.SetActive(true);
                break;
            case 5:
                _indication1.gameObject.SetActive(true);
                _indication2.gameObject.SetActive(true);
                _indication3.gameObject.SetActive(true);
                _indication4.gameObject.SetActive(true);
                _indication5.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void SetIndicationsText(AssemblyStep step)
    {
        int counter = 1;
        foreach (Indication indic in step.Indications)
        {
            string text = indic.IndicationText;
            string indicationText = $"<size=8>Indication {counter}</size>\n";
            indicationText += $"<size=6><alpha=#88>{text}</size>";
            switch (counter)
            {
                case 1:
                    _indication1.GetComponentInChildren<TextMeshProUGUI>().text = indicationText;
                    break;
                case 2:
                    _indication2.GetComponentInChildren<TextMeshProUGUI>().text = indicationText;
                    break;
                case 3:
                    _indication3.GetComponentInChildren<TextMeshProUGUI>().text = indicationText;
                    break;
                case 4:
                    _indication4.GetComponentInChildren<TextMeshProUGUI>().text = indicationText;
                    break;
                case 5:
                    _indication5.GetComponentInChildren<TextMeshProUGUI>().text = indicationText;
                    break;
                default:
                    break;
            };
            counter++;
        }
    }

    private void SetStepIllustration(AssemblyStep step)
    {
        Sprite illustration = step.StepIllustration;
        _stepImage.sprite = illustration;
    }
}