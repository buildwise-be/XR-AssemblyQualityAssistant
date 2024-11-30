using System;

namespace _project.Scripts.Entities
{
    public class AssemblyProcessDataEntity
    {
        public AssemblyStepData[] m_assemblySteps;
        public string m_projectId;

        private AssemblyProcessDataEntity()
        {
            
        }
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
            var result = m_assemblySteps[currentStepIndex].m_remarks;
            if(result == null) result = Array.Empty<AssemblyRemark>();
            return result;
        }

        public void SetStepRemarks(int currentStepIndex, AssemblyRemark[] toArray)
        {
            m_assemblySteps[currentStepIndex].m_remarks = toArray;
        }

        public void StartStep(int index)
        {
            m_assemblySteps[index].m_nbSessions++;
        }

        public class Builder
        {
            private string projectId;
            private int numberOfSteps;

            public Builder Create(string projectId)
            {
                this.projectId = projectId;
                return this;
            }

            public Builder WithNumberOfSteps(int numberOfSteps)
            {
                this.numberOfSteps = numberOfSteps;
                return this;
            }

            public AssemblyProcessDataEntity Build()
            {
                var assemblyProcessDataEntity = new AssemblyProcessDataEntity();
                assemblyProcessDataEntity.m_projectId = this.projectId;
                assemblyProcessDataEntity.m_assemblySteps = new AssemblyStepData[this.numberOfSteps];
                for (int i = 0; i < this.numberOfSteps; i++)
                {
                    assemblyProcessDataEntity.m_assemblySteps[i] = new AssemblyStepData();
                }
                return assemblyProcessDataEntity;
            }
            
            
        }
    }
}