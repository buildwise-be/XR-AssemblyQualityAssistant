using UnityEngine;

public class QualityAssistantSceneBootstrap : MonoBehaviour
{
    private HandMenuActions _handMenuActions;
    public AssemblyProcessController _assemblyProcessController;
    public DictationPanelController _dictationPanelController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var monitor = new AssemblyStepFeedbackMonitor();
        _handMenuActions = FindFirstObjectByType<HandMenuActions>();
        _assemblyProcessController.SetMonitor(monitor);
        _dictationPanelController.SetUseCase(monitor);
    }
}
