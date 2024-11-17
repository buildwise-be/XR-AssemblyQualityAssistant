namespace _project.Scripts.UseCases
{
    public struct SimpleStepDataDto : IStepDataDto
    {
        public float[] StepDuration { get; set; }
        public int[] StepSessions { get; set; }
        public int Length { get; set; }
    }
}