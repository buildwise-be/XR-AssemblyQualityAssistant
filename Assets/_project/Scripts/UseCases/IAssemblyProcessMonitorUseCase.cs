using System;
using System.Collections.Generic;

namespace _project.Scripts.UseCases
{
    public interface IAssemblyProcessMonitorUseCase
    {
        void StartStepMonitoring(int index, float time);
        void EndStepMonitoring(int index,float time);
        void EndMonitoring();
        void AddAssemblyRemark(string data);
        void InitMonitoring(string projectId);
    
        Action OnStartDictationProcess { get; set; }
        Action OnStartAssemblyProcessEvent { get; set; }
        Action OnShowAssemblyPanel { get; set; }
        Action OnPrematureAssemblyEnd { get; set; }
        IRemarkDto GetCurrentStepRemarkList();
        void StopDictation();
        void InitializeDictationForRemarkReporting();
        void InitializeDictationForIssueReporting();
        IStepDataDto GetAssemblyProcessData();
        bool IsInIssueMode { get; }
    }
}