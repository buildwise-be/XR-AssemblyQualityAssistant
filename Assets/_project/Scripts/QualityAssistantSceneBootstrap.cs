using System;
using _project.Scripts.Controllers;
using _project.Scripts.Gateways;
using _project.Scripts.UseCases;
using UnityEngine;

public class QualityAssistantSceneBootstrap : MonoBehaviour
{
    [SerializeField] private AssemblyProjectScriptableObject _currentProject;
    private HandMenuActions _handMenuActions;
    public AssemblyProcessController _assemblyProcessController;
    public DictationPanelController _dictationPanelController;
    public AssemblyProcessView _assemblyProcessView;

    private AssemblyStepFeedbackMonitor _monitor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _monitor = new AssemblyStepFeedbackMonitor(new FakeDataLoader());
        _handMenuActions = FindFirstObjectByType<HandMenuActions>();
        _assemblyProcessController.SetMonitor(_monitor);
        _dictationPanelController.SetUseCase(_monitor);
        
        _assemblyProcessView.SetController(_assemblyProcessController);
        
    }

    private void Start()
    {
        _monitor.InitMonitoring(_currentProject.m_guid);
    }
}