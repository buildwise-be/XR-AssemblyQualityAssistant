using System;
using System.Collections.Generic;
using _project.Scripts.Entities;

public class AssemblyStepFeedbackMonitor : IAssemblyProcessMonitorUseCase
{

    public AssemblyStepFeedbackMonitor()
    {
        
    }
    private AssemblyStepFeedback[] _assemblyStepFeedbacks;
    private int _currentStepIndex;

    public void StartStepMonitoring(int index,float time)
    {
        _currentStepIndex = index;
        _assemblyStepFeedbacks[_currentStepIndex] = new AssemblyStepFeedback(time);
    }

    public void EndStepMonitoring(int index, float time)
    {
        _assemblyStepFeedbacks[index].Close(time);
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
        _assemblyStepFeedbacks[currentStepIndex]
    }

    public void InitMonitoring(int _nbOfSteps)
    {
        _assemblyStepFeedbacks = new AssemblyStepFeedback[_nbOfSteps];
    }

    public void InitializeDictation()
    {
        OnStartDictationProcess.Invoke();
    }

    public Action OnStartDictationProcess { get; set; }
    public void SaveMessage(string message)
    {
        
    }

    public void SaveMessages(List<string> _data)
    {
        throw new NotImplementedException();
    }
}