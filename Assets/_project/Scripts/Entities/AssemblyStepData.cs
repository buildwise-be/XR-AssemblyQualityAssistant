using System;
using System.Linq;
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

        public int GetNumberOfEntries(AssemblyRemark.TYPE type)
        {
            return m_remarks.Count(t => t.m_type == type);
        }
    }
}