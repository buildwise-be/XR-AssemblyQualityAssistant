namespace _project.Scripts.Entities
{
    public class AssemblyProcessDataEntity
    {
        public AssemblyStepData[] m_assemblySteps;
        public string m_projectId;

        public AssemblyProcessDataEntity(string projectId)
        {
            m_projectId = projectId;
        }
        
        public void SetStepDuration(int index, float time)
        {
            m_assemblySteps[index].m_duration+= time;
        }

        public AssemblyRemark[] GetStepRemarks(int currentStepIndex)
        {
            return m_assemblySteps[currentStepIndex].m_remarks;
        }

        public void SetStepRemarks(int currentStepIndex, AssemblyRemark[] toArray)
        {
            m_assemblySteps[currentStepIndex].m_remarks = toArray;
        }

        public void StartStep(int index)
        {
            m_assemblySteps[index].m_nbSessions++;
        }
    }
}