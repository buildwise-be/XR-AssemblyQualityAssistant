using System;
using System.Collections.Generic;
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
            if(m_remarks == null) return 0;
            return m_remarks.Count(t => t.m_type == type);
        }
        
        public string[] GetEntries(AssemblyRemark.TYPE type)
        {
            if(m_remarks == null) return Array.Empty<string>();
            return (from entry in m_remarks where entry.m_type == type select entry.m_message).ToArray();
        }
    }
} 