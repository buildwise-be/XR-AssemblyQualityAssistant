namespace _project.Scripts.Controllers
{
    public struct AssemblyProcessDataDto
    {
        public int DataNbOfRemarks { get; }
        public float m_stepDuration;
        public int m_nbOfStepSession;
        public int m_dataNbOfIssues { get; }
        
        public AssemblyProcessDataDto(float f, int dataStepSession, int dataNbOfIssues, int dataNbOfRemarks)
        {
            DataNbOfRemarks = dataNbOfRemarks;
            m_stepDuration = f;
            m_nbOfStepSession = dataStepSession;
            m_dataNbOfIssues = dataNbOfIssues;
        }
    }
}