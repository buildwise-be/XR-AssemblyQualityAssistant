namespace _project.Scripts.Controllers
{
    public struct AssemblyProcessDataDto
    {
        public float m_stepDuration;
        public int m_nbOfStepSession;

        public AssemblyProcessDataDto(float f, int dataStepSession)
        {
            m_stepDuration = f;
            m_nbOfStepSession = dataStepSession;
        }
    }
}