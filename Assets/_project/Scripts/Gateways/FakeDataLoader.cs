using _project.Scripts.Entities;
using UnityEngine;

namespace _project.Scripts.Gateways
{
    public class FakeDataLoader : IFeedbackDataLoaderGateway
    {
        private readonly int _stepCount;
        private string[] _comments;

        public FakeDataLoader(int fakeLoaderStepCount)
        {
            _stepCount = fakeLoaderStepCount;
            _comments = new []
            {
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque convallis lacus et cursus efficitur. Integer sed laoreet turpis. Nulla felis sem, mattis sed dolor quis, suscipit eleifend nisi.",
                "Nulla luctus facilisis auctor. Aenean ornare risus nec ipsum mollis, et sodales velit pellentesque.",
                "Integer dignissim mattis mauris, eu feugiat ante luctus a. Donec in maximus nisi.",
                "Vestibulum interdum commodo enim ultricies tristique. Proin sem augue, tincidunt nec risus at, auctor sodales purus.",
                "Integer dignissim mattis mauris, eu feugiat ante luctus a. Donec in maximus nisi."
            };
        }

        public AssemblyProcessDataEntity GetAssemblyData(string projectId)
        {
            var data = new AssemblyProcessDataEntity(projectId);

            data.m_assemblySteps = new AssemblyStepData[_stepCount];
            for (var i = 0; i < _stepCount; i++)
            {
                var assemblyRemarks = GenerateRandomRemarks();
                data.m_assemblySteps[i] = new AssemblyStepData(assemblyRemarks);
            }

            return data;
        }

        private AssemblyRemark[] GenerateRandomRemarks()
        {
            var result = new AssemblyRemark[Random.Range(0, 4)];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new AssemblyRemark(
                    Mathf.RoundToInt(Random.value)==0? AssemblyRemark.TYPE.ISSUE: AssemblyRemark.TYPE.REMARK, 
                    _comments[Random.Range(0,_comments.Length)]);
            }

            return result;
        }

        public void SaveData(AssemblyProcessDataEntity assemblyStepFeedbacks)
        {
            var json = JsonUtility.ToJson(assemblyStepFeedbacks,true);
            Debug.Log(json);
        }
    }
}