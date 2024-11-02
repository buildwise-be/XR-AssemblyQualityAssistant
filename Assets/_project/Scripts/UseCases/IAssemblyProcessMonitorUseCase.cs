using System;
using System.Collections.Generic;

public interface IAssemblyProcessMonitorUseCase
{
    void StartStepMonitoring(int index, float time, out  _currentStepIndex);
    void EndStepMonitoring(int index,float time);
    void EndMonitoring(float time);
    void AddRemark(int currentStepIndex, string data);
    void InitMonitoring(int numberOfSteps);
    void InitializeDictation();
    
    Action OnStartDictationProcess { get; set; }
    void SaveMessage(string message);
    void SaveMessages(List<string> _data);
}