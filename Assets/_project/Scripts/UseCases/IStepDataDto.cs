namespace _project.Scripts.UseCases
{
    public interface IStepDataDto
    {
        float[] StepDuration { get; set; }
        int[] StepSessions { get; set; }
        int Length { get; set; }
    }
}