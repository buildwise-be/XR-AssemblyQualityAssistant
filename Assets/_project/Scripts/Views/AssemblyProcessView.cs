using System;
using System.Collections.Generic;
using System.Reflection;
using _project.Scripts.Controllers;
using _project.Scripts.Views;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class AssemblyProcessView : MonoBehaviour
{
    [SerializeField] private GameObject _instructionPrebab;
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private Image _illustration;
    [SerializeField] private Transform _indicationsContainer;
    [SerializeField] private GameObject _previousStepButton;
    [SerializeField] private NexStepButtonBaseView _nextStepButton;
    
    private IAssemblyProcessController _controller;

    private void Start()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLanguageChanged;
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLanguageChanged;
    }

    private void OnLanguageChanged(Locale obj)
    {
        _controller.ReloadCurrentStep();
    }

    public void SetController(IAssemblyProcessController controller)
    {
        _controller = controller;
        _controller.OnDisplayStep += DisplayStepInfo;  
        _controller.OnShowPanel += ShowPanel;  
        _controller.OnEndProcessEvent += ClosePanel;  
    }

    private void ClosePanel(bool isShow)
    {
        gameObject.SetActive(false);
    }

    private void ShowPanel()
    {
        gameObject.SetActive(true);
        transform.SetPositionAndRotation(Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Camera.main.transform.rotation);

    }



    private void DisplayStepInfo(int i,AssemblyStep step)
    {
        var nbOfSteps = _controller.TotalNumberOfSteps;
        if (i == nbOfSteps - 1)
        {
            _nextStepButton.SetEndOfProcessText();
        }
        else
        {
            _nextStepButton.SetDefaultText();
        }
       
        gameObject.SetActive(true);
        ClearContent();
        UpdateIllustration(step.StepIllustration);
        _headerText.SetText($"#{i+1+"-"+nbOfSteps}: {step.GetTitle(LocalizationSettings.SelectedLocale.LocaleName)}");
        UpdateInstructions(step.Indications);
        _previousStepButton.SetActive(i > 0);
    }

    private void ClearContent()
    {
        for (int i = _indicationsContainer.childCount-1; i >=0 ; i--)
        {
            if(_indicationsContainer.GetChild(i).GetComponent<IAssemblyInsctructionView>()!=null) Destroy(_indicationsContainer.GetChild(i).gameObject);
        }
    }

    private void UpdateInstructions(List<Indication> indications)
    {
        var currentLanguage = LocalizationSettings.SelectedLocale.LocaleName;
        for (int i = 0; i<indications.Count; i++)
        {
            var instructionInstance = Instantiate(_instructionPrebab, _indicationsContainer);
            var instructionView = instructionInstance.GetComponent<IAssemblyInsctructionView>();
            instructionView.SetText(i,indications[i].GetText(currentLanguage));
        }
    }

    private void UpdateIllustration(Sprite stepIllustration)
    {
        _illustration.sprite = stepIllustration;
    }


    public void GoToPreviousStep()
    {
        _controller.GoToPreviousStep();
    }

    private void OnDisable()
    {
        //_controller.OnDisplayStep -= DisplayStepInfo;
    }

    public void DisplayStep(int index)
    {
        
    }

    public void ValidateStep()
    {
        CloseCurrentView();
        _controller.ValidateStep();
    }

    private void CloseCurrentView()
    {
        
    }

    public void OnOpenDictationButtonClick()
    {
        _controller.OpenDictationPanelForRemarkReporting();
        gameObject.SetActive(false);
    }

    public void OnOpenDictationButtonClickForIssue()
    {
        _controller.OpenDictationPanelForIssueReporting();
        gameObject.SetActive(false);
    }
}