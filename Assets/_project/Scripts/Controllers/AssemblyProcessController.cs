using System;
using _project.Scripts.UseCases;
using UnityEngine;

public class AssemblyProcessController : MonoBehaviour, IAssemblyProcessController
{
    public Action<AssemblyStep> OnDisplayStep {  get; set; }
    [SerializeField] private AppData _appData;

    private AssemblyProjectScriptableObject _currentProject;
    private int _currentStepIndex;
    private float _stepStartTime;

    private IAssemblyProcessMonitorUseCase _assemblyProcessMonitorUseCase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartAssembly();
        
    }
    
    public void SetMonitor(IAssemblyProcessMonitorUseCase monitor)
    {
        _assemblyProcessMonitorUseCase = monitor;
    }

    public void StartAssembly()
    {
        _currentProject = _appData.project;
        _currentStepIndex = -1;
        _assemblyProcessMonitorUseCase.InitMonitoring(_currentProject.m_guid);
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
        var step = _currentProject.GetStep(_i);
        OnDisplayStep?.Invoke(step);
    }
    
    public void ValidateStep()
    {
        _assemblyProcessMonitorUseCase.EndStepMonitoring(_currentStepIndex, Time.time);
    }

    public void OpenDictationPanel()
    {
        _assemblyProcessMonitorUseCase.InitializeDictation();
    }
    
}