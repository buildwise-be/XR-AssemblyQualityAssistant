using _project.Scripts.Entities;
using UnityEngine;

namespace _project.Scripts.Gateways
{
    public class FakeDataLoader : IFeedbackDataLoaderGateway
    {
        public AssemblyProcessDataEntity GetAssemblyData(string projectId)
        {
            var data = new AssemblyProcessDataEntity(projectId);
            
            var remarks_01 = new[]
            {
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque convallis lacus et cursus efficitur. Integer sed laoreet turpis. Nulla felis sem, mattis sed dolor quis, suscipit eleifend nisi."),
                new AssemblyRemark(AssemblyRemark.TYPE.ISSUE, "Nulla luctus facilisis auctor. Aenean ornare risus nec ipsum mollis, et sodales velit pellentesque."),
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "Vestibulum interdum commodo enim ultricies tristique. Proin sem augue, tincidunt nec risus at, auctor sodales purus."),
                new AssemblyRemark(AssemblyRemark.TYPE.ISSUE, "Vestibulum interdum commodo enim ultricies tristique. Proin sem augue, tincidunt nec risus at, auctor sodales purus."),
            };
            
            var remarks_02 = new[]
            {
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "Integer ornare viverra maximus. Suspendisse eget auctor enim."),
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "Duis ut diam et lacus pretium eleifend eget et mauris. Aliquam sed lacinia turpis, a auctor arcu. In venenatis non orci at pretium. Suspendisse sit amet congue nulla, et finibus mi.")
            };
            
            var remarks_03 = new[]
            {
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "Phasellus eros odio, tincidunt eget leo eget, venenatis pretium metus. Donec dignissim ligula lorem, eget blandit lectus bibendum et."),
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "Integer dignissim mattis mauris, eu feugiat ante luctus a. Donec in maximus nisi.")
            };            
            var remarks = new[]
            {
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "This is a message from the loader"),
                new AssemblyRemark(AssemblyRemark.TYPE.REMARK, "This is a second message from the loader")
            };
            
            data.m_assemblySteps = new[]
            {
                new AssemblyStepData(remarks_01),
                new AssemblyStepData(remarks_02),
                new AssemblyStepData(remarks_03),
                new AssemblyStepData(remarks),
                new AssemblyStepData(remarks),
                new AssemblyStepData(remarks),
                new AssemblyStepData(remarks),
                new AssemblyStepData(remarks),
                new AssemblyStepData(remarks)
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