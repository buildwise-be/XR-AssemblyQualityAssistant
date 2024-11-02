using _project.Scripts.Entities;

namespace _project.Scripts.Gateways
{
    public class FakeDataLoader : IFeedbackDataLoaderGateway
    {
        public AssemblyStepFeedback[] GetAssemblyData(string projectId)
        {
            var remarks = new[]
            {
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "This is a message from the loader"),
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "This is a second message from the loader")
            };
            return new[]
            {
                new AssemblyStepFeedback(10, remarks)
            };
        }
    }
}