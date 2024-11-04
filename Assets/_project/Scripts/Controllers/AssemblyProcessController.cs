using System;
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

    private void LoadStep(int _i)
    {
        _assemblyProcessMonitorUseCase.StartStepMonitoring(_i, Time.time);
        _currentStep = _currentProject.GetStep(_i);
        OnDisplayStep?.Invoke(_i,_currentStep);
    }
    
    public void ValidateStep()
    {
        _assemblyProcessMonitorUseCase.EndStepMonitoring(_currentStepIndex, Time.time);
        LoadNextStep();
    }

    public void OpenDictationPanel()
    {
        _assemblyProcessMonitorUseCase.InitializeDictation();
        
    }

    public void GoToPreviousStep()
    {
        _currentStepIndex--;
        LoadStep(_currentStepIndex);
    }
}