using _project.Scripts.Entities;

namespace _project.Scripts.Gateways
{
    public interface IFeedbackDataLoaderGateway
    {
        AssemblyProcessDataEntity GetAssemblyData(string projectId);
        void SaveData(AssemblyProcessDataEntity assemblyStepFeedbacks);
    }
}