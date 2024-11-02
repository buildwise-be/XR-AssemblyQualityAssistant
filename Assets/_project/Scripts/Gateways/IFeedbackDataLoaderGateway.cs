using _project.Scripts.Entities;

namespace _project.Scripts.Gateways
{
    public interface IFeedbackDataLoaderGateway
    {
        AssemblyStepFeedback[] GetAssemblyData(string projectId);
    }
}