using System;
using _project.Scripts.Entities;

public class AssemblyStepFeedbackMonitor : IAssemblyProcessMonitorUseCase
{
    private AssemblyStepFeedback[] _assemblyStepFeedbacks;
    public void StartStepMonitoring(int index,float time)
    {
        
    }

    public void EndStepMonitoring(int index, float time)
    {
        throw new NotImplementedException();
    }

    public void StartStepMonitoring(float time)
    {
        throw new NotImplementedException();
    }

    public void EndMonitoring(float time)
    {
        throw new NotImplementedException();
    }

    public void AddRemark(int currentStepIndex, string data)
    {
        throw new NotImplementedException();
    }

    public void InitMonitoring(int _nbOfSteps)
    {
        
    }

    public void InitializeDictation()
    {
        OnStartDictationProcess.Invoke();
    }

    public Action OnStartDictationProcess { get; set; }
    public void SaveMessage(string message)
    {
        
    }
}