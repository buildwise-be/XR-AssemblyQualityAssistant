using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Entities;
using _project.Scripts.Gateways;

namespace _project.Scripts.UseCases
{
    public class AssemblyStepFeedbackMonitor : IAssemblyProcessMonitorUseCase
    {
        private AssemblyStepFeedback[] _assemblyStepFeedbacks;
        private int _currentStepIndex;
        private readonly IFeedbackDataLoaderGateway _feedbackDataLoader;
        
        public AssemblyStepFeedbackMonitor(IFeedbackDataLoaderGateway feedbackDataLoader)
        {
            _feedbackDataLoader = feedbackDataLoader;
        }
        

        public void StartStepMonitoring(int index,float time)
        {
            _currentStepIndex = index;
            _assemblyStepFeedbacks[_currentStepIndex] = new AssemblyStepFeedback(time);
        }

        public void EndStepMonitoring(int index, float time)
        {
            _assemblyStepFeedbacks[index].Close(time);
        }

        public void EndMonitoring(float time)
        {
            throw new NotImplementedException();
        }
        
        public void InitMonitoring(string projectId)
        {
            _assemblyStepFeedbacks = _feedbackDataLoader.GetAssemblyData(projectId);
        }

        public void InitializeDictation()
        {
            OnStartDictationProcess.Invoke();
        }

        public Action OnStartDictationProcess { get; set; }
        public IRemarkDto GetCurrentStepRemarkList()
        {
            var data = new string[_assemblyStepFeedbacks[_currentStepIndex].m_remarks.Length];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = _assemblyStepFeedbacks[_currentStepIndex].m_remarks[i].m_message;
            }
            return new RemarkData(data); 
        }

        public void AddRemark(string message)
        {
            var listOfPreviousRemarks = _assemblyStepFeedbacks[_currentStepIndex].m_remarks.ToList();
            listOfPreviousRemarks.Add(new AssemblyRemark(AssemblyRemark.TYPE.REMARK,message) );
            _assemblyStepFeedbacks[_currentStepIndex].m_remarks = listOfPreviousRemarks.ToArray();
        }
        
        public void AddIssue(string message)
        {
            var listOfPreviousRemarks = _assemblyStepFeedbacks[_currentStepIndex].m_remarks.ToList();
            listOfPreviousRemarks.Add(new AssemblyRemark(AssemblyRemark.TYPE.ISSUE,message) );
            _assemblyStepFeedbacks[_currentStepIndex].m_remarks = listOfPreviousRemarks.ToArray();
        }

        public void SaveMessages(List<string> data)
        {
            
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