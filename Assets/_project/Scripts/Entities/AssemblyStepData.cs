using System;

namespace _project.Scripts.Entities
{
    [Serializable]
    public class AssemblyStepData
    {
        public float m_startTime;
        public float m_endTime;
        public AssemblyRemark[] m_remarks;
        
        public AssemblyStepData()
        {
            
        }
        
        public AssemblyStepData(float startTime, AssemblyRemark[] remarks)
        {
            m_startTime = startTime;
            m_remarks = remarks;
        }
    }
}