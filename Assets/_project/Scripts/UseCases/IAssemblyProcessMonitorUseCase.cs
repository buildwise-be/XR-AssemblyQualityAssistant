using System;
using System.Collections.Generic;

namespace _project.Scripts.UseCases
{
    public interface IAssemblyProcessMonitorUseCase
    {
        void StartStepMonitoring(int index, float time);
        void EndStepMonitoring(int index,float time);
        void EndMonitoring(float time);
        void AddRemark(string data);
        void InitMonitoring(string projectId);
        void InitializeDictation();
    
        Action OnStartDictationProcess { get; set; }
        IRemarkDto GetCurrentStepRemarkList();
    }
}