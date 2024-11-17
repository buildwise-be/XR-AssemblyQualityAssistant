using System;
using UnityEngine.Serialization;

namespace _project.Scripts.Entities
{
    [Serializable]
    public class AssemblyStepData
    {
        public float m_duration;
        public AssemblyRemark[] m_remarks;
        public int m_nbSessions;

        public AssemblyStepData()
        {
            
        }
        
        public AssemblyStepData(AssemblyRemark[] remarks)
        {
            m_remarks = remarks;
        }
    }
}