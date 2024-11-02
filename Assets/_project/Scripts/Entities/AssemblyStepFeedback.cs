using System;

namespace _project.Scripts.Entities
{
    [Serializable]
    public class AssemblyStepFeedback
    {
        private readonly int _index;
        public readonly float _startTime;

        public AssemblyStepFeedback(float _startTime)
        {
            _startTime = _startTime;
        }

        public void Close(float _time)
        {
            throw new NotImplementedException();
        }
    }
}