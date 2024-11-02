using _project.Scripts.Controllers;
using _project.Scripts.Gateways;
using _project.Scripts.UseCases;
using UnityEngine;

public class QualityAssistantSceneBootstrap : MonoBehaviour
{
    private HandMenuActions _handMenuActions;
    public AssemblyProcessController _assemblyProcessController;
    public DictationPanelController _dictationPanelController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var monitor = new AssemblyStepFeedbackMonitor(new FakeDataLoader());
        _handMenuActions = FindFirstObjectByType<HandMenuActions>();
        _assemblyProcessController.SetMonitor(monitor);
        _dictationPanelController.SetUseCase(monitor);
    }
}