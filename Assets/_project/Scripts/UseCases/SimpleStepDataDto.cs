using System.Collections.Generic;

namespace _project.Scripts.UseCases
{
    public struct SimpleStepDataDto : IStepDataDto
    {
        public List<string[]> Remarks { get; set; }
        public List<string[]> Issues { get; set; }
        public float[] StepDuration { get; set; }
        public int[] StepSessions { get; set; }
        public int Length { get; set; }
        public int[] nbOfIssues { get; set; }
        public int[] nbOfRemarks { get; set; }
    }
}