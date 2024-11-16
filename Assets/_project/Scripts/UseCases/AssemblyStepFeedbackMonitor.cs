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
        private AssemblyRemark.TYPE _currentReportType;

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
            var types = new IRemarkDto.RemarkType[remarks.Length];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = remarks[i].m_message;
                types[i] = remarks[i].m_type == AssemblyRemark.TYPE.REMARK
                    ? IRemarkDto.RemarkType.Remark
                    : IRemarkDto.RemarkType.Issue;
            }
            return new RemarkData(data, types); 
        }

        public void StopDictation()
        {
            OnShowAssemblyPanel.Invoke();
        }

        public void InitializeDictationForRemarkReporting()
        {
            _currentReportType = AssemblyRemark.TYPE.REMARK;
            OnStartDictationProcess.Invoke();
        }

        public void InitializeDictationForIssueReporting()
        {
            _currentReportType = AssemblyRemark.TYPE.REMARK;
            OnStartDictationProcess.Invoke();
        }

        public void AddAssemblyRemark(string message)
        {
            var listOfPreviousRemarks = _assemblyProcessDataEntity.GetStepRemarks(_currentStepIndex).ToList();
            listOfPreviousRemarks.Add(new AssemblyRemark(_currentReportType,message));
            _assemblyProcessDataEntity.SetStepRemarks(_currentStepIndex, listOfPreviousRemarks.ToArray());
            _feedbackDataLoader.SaveData(_assemblyProcessDataEntity);
        }
    }

    public readonly struct RemarkData : IRemarkDto
    {
        private readonly string[] _messages;
        private readonly IRemarkDto.RemarkType[] _types;

        public RemarkData(string[] remarkMessage, IRemarkDto.RemarkType[] types)
        {
            _messages = remarkMessage;
            _types = types;
        }

        public string[] GetMessages()
        {
            return _messages;
        }

        public IRemarkDto.RemarkType[] GetRemarkType()
        {
            return _types;
        }
    }
}