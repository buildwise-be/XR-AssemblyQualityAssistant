using System;

public interface IAssemblyProcessMonitorUseCase
{
    void StartStepMonitoring(int index,float time);
    void EndStepMonitoring(int index,float time);
    void EndMonitoring(float time);
    void AddRemark(int currentStepIndex, string data);
    void InitMonitoring(int numberOfSteps);
    void InitializeDictation();
    
    Action OnStartDictationProcess { get; set; }
    void SaveMessage(string message);
}