using System;

namespace _project.Scripts.Entities
{
    [Serializable]
    public class AssemblyStepFeedback
    {
        private readonly float _startTime;
        public AssemblyRemark[] m_remarks;
        
        public AssemblyStepFeedback(float startTime)
        {
            _startTime = startTime;
        }
        
        public AssemblyStepFeedback(float startTime, AssemblyRemark[] remarks)
        {
            _startTime = startTime;
            m_remarks = remarks;
        }

        public void Close(float time)
        {
            throw new NotImplementedException();
        }
    }
}