using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyProcessView : MonoBehaviour
{
    [SerializeField] private GameObject _instructionPrebab;
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private Image _illustration;
    [SerializeField] private Transform _indicationsContainer;

    private IAssemblyProcessController _controller;
    [SerializeField] private GameObject _previousStepButton;

    public void SetController(IAssemblyProcessController controller)
    {
        _controller = controller;
        _controller.OnDisplayStep += DisplayStepInfo;  
        _controller.OnShowPanel += ShowPanel;  
    }

    private void ShowPanel()
    {
        gameObject.SetActive(true);
    }



    private void DisplayStepInfo(int i,AssemblyStep step)
    {
        gameObject.SetActive(true);
        ClearContent();
        UpdateIllustration(step.StepIllustration);
        _headerText.SetText($"#{i+1}: {step.Title}");
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
        for (int i = 0; i<indications.Count; i++)
        {
            var instructionInstance = Instantiate(_instructionPrebab, _indicationsContainer);
            var instructionView = instructionInstance.GetComponent<IAssemblyInsctructionView>();
            instructionView.SetText(i,indications[i].IndicationText);
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
        _controller.OpenDictationPanel();
        gameObject.SetActive(false);
    }
}
