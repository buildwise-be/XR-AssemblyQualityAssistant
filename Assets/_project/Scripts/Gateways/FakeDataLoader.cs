using _project.Scripts.Entities;
using UnityEngine;

namespace _project.Scripts.Gateways
{
    public class FakeDataLoader : IFeedbackDataLoaderGateway
    {
        public AssemblyProcessDataEntity GetAssemblyData(string projectId)
        {
            var data = new AssemblyProcessDataEntity(projectId);
            
            var remarks = new[]
            {
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "This is a message from the loader"),
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "This is a second message from the loader")
            };
            
            data.m_assemblySteps = new[]
            {
                new AssemblyStepData(10, remarks),
                new AssemblyStepData(10, remarks),
                new AssemblyStepData(10, remarks),
                new AssemblyStepData(10, remarks),
                new AssemblyStepData(10, remarks),
                new AssemblyStepData(10, remarks),
                new AssemblyStepData(10, remarks),
                new AssemblyStepData(10, remarks),
                new AssemblyStepData(10, remarks)
                
            };
            return data;
            
            
        }

        public void SaveData(AssemblyProcessDataEntity assemblyStepFeedbacks)
        {
            var json = JsonUtility.ToJson(assemblyStepFeedbacks,true);
            Debug.Log(json);
        }
    }
}