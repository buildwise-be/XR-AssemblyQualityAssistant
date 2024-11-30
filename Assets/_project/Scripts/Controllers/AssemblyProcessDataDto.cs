using System.Collections.Generic;

namespace _project.Scripts.Controllers
{
    public struct AssemblyProcessDataDto
    {
        public int DataNbOfRemarks { get; }
        public float m_stepDuration;
        public int m_nbOfStepSession;
        public readonly string[] m_dataIssues;
        public readonly string[] m_dataRemarks;
        public int m_dataNbOfIssues { get; }
        
        public AssemblyProcessDataDto(float f, int dataStepSession, int dataNbOfIssues, int dataNbOfRemarks, string[] dataIssues, string[] dataRemarks)
        {
            DataNbOfRemarks = dataNbOfRemarks;
            m_stepDuration = f;
            m_nbOfStepSession = dataStepSession;
            m_dataIssues = dataIssues;
            m_dataRemarks = dataRemarks;
            m_dataNbOfIssues = dataNbOfIssues;
        }
    }
}