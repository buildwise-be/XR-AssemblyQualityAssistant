using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Entities;
using _project.Scripts.Gateways;

namespace _project.Scripts.UseCases
{
    public class AssemblyStepFeedbackMonitor : IAssemblyProcessMonitorUseCase
    {
        private AssemblyProcessDataEntity _assemblyProcessDataEntity;
        private int _currentStepIndex;
        private readonly IFeedbackDataLoaderGateway _feedbackDataLoader;
        
        public Action OnStartDictationProcess { get; set; }
        public Action OnStartAssemblyProcessEvent { get; set; }
        public Action OnShowAssemblyPanel { get; set; }

        public AssemblyStepFeedbackMonitor(IFeedbackDataLoaderGateway feedbackDataLoader)
        {
            _feedbackDataLoader = feedbackDataLoader;
        }
        
        public void InitMonitoring(string projectId)
        {
            _assemblyProcessDataEntity = _feedbackDataLoader.GetAssemblyData(projectId);
            OnStartAssemblyProcessEvent?.Invoke();
        }

        public void StartStepMonitoring(int index,float time)
        {
            _currentStepIndex = index;
            _assemblyProcessDataEntity.SetStepStartTime(_currentStepIndex,time);
        }

        public void EndStepMonitoring(int index, float time)
        {
            _assemblyProcessDataEntity.SetStepEndTime(index,time);
            _feedbackDataLoader.SaveData(_assemblyProcessDataEntity);
        }

        public void EndMonitoring(float time)
        {
            throw new NotImplementedException();
        }
        
       

        public void InitializeDictation()
        {
            OnStartDictationProcess.Invoke();
        }

        
        public IRemarkDto GetCurrentStepRemarkList()
        {
            var remarks = _assemblyProcessDataEntity.GetStepRemarks(_currentStepIndex);
            var data = new string[remarks.Length];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = remarks[i].m_message;
            }
            return new RemarkData(data); 
        }

        public void StopDictation()
        {
            OnShowAssemblyPanel.Invoke();
        }

        public void AddRemark(string message)
        {
            var listOfPreviousRemarks = _assemblyProcessDataEntity.GetStepRemarks(_currentStepIndex).ToList();
            listOfPreviousRemarks.Add(new AssemblyRemark(AssemblyRemark.TYPE.REMARK,message));
            _assemblyProcessDataEntity.SetStepRemarks(_currentStepIndex, listOfPreviousRemarks.ToArray());
            _feedbackDataLoader.SaveData(_assemblyProcessDataEntity);
        }
        
        public void AddIssue(string message)
        {
            var listOfPreviousRemarks = _assemblyProcessDataEntity.GetStepRemarks(_currentStepIndex).ToList();
            listOfPreviousRemarks.Add(new AssemblyRemark(AssemblyRemark.TYPE.ISSUE,message) );
            _assemblyProcessDataEntity.SetStepRemarks(_currentStepIndex, listOfPreviousRemarks.ToArray());
        }
        
    }

    public readonly struct RemarkData : IRemarkDto
    {
        private readonly string[] _messages;

        public RemarkData(string[] remarkMessage)
        {
            _messages = remarkMessage;
        }

        public string[] GetMessages()
        {
            return _messages;
        }
    }
}