using System;
using _project.Scripts.Controllers;
using _project.Scripts.UseCases;
using UnityEngine;

public class AssemblyProcessController : MonoBehaviour, IAssemblyProcessController
{
    public Action<int, AssemblyStep> OnDisplayStep {  get; set; }
    public Action OnShowPanel { get; set; }
    [SerializeField] private AppData _appData;

    private AssemblyProjectScriptableObject _currentProject;
    private int _currentStepIndex;
    private float _stepStartTime;

    private IAssemblyProcessMonitorUseCase _assemblyProcessMonitorUseCase;

    private AssemblyStep _currentStep;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartAssembly();
        
    }
    
    public void SetMonitor(IAssemblyProcessMonitorUseCase monitor)
    {
        _assemblyProcessMonitorUseCase = monitor;
        _currentProject = _appData.project;
        _currentStepIndex = -1;
        _assemblyProcessMonitorUseCase.OnStartAssemblyProcessEvent += StartAssembly;
        _assemblyProcessMonitorUseCase.OnShowAssemblyPanel += ShowPanel;
    }

    private void ShowPanel()
    {
        OnShowPanel?.Invoke();
    }

    private void StartAssembly()
    {
        LoadFirstStep();
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextStep();

        }
#endif

    }

    public void LoadFirstStep()
    {
        _currentStepIndex = 0;
        LoadStep(_currentStepIndex);
    }

    public void LoadNextStep()
    {
        _currentStepIndex++;
        if (_currentStepIndex >= _currentProject.StepsCount)
        {
            
        }
        else
        {
            LoadStep(_currentStepIndex);
        }
        
        
    }

    private void LoadStep(int index)
    {
        
        _assemblyProcessMonitorUseCase.StartStepMonitoring(index, Time.time);
        _currentStep = _currentProject.GetStep(index);
        OnDisplayStep?.Invoke(index,_currentStep);
    }

    public int TotalNumberOfSteps => _currentProject.StepsCount;

    public void ValidateStep()
    {
        _assemblyProcessMonitorUseCase.EndStepMonitoring(_currentStepIndex, Time.time);
        LoadNextStep();
    }
    

    public void GoToPreviousStep()
    {
        _currentStepIndex--;
        LoadStep(_currentStepIndex);
    }

    public void OpenDictationPanelForRemarkReporting()
    {
        _assemblyProcessMonitorUseCase.InitializeDictationForRemarkReporting();
    }

    public void OpenDictationPanelForIssueReporting()
    {
        _assemblyProcessMonitorUseCase.InitializeDictationForIssueReporting();
    }
}