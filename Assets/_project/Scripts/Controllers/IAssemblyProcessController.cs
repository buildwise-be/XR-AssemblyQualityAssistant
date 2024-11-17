using System;
using _project.Scripts.Controllers;

public interface IAssemblyProcessController
{
    public Action<int,AssemblyStep> OnDisplayStep { get; set; }
    public Action OnShowPanel { get; set; }
    int TotalNumberOfSteps { get; }
    Action OnEndProcessEvent { get; set; }

    void ValidateStep();
    void GoToPreviousStep();
    void OpenDictationPanelForRemarkReporting();
    void OpenDictationPanelForIssueReporting();
    AssemblyProcessDataDto[] GetAssemblyProcessData();
}

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