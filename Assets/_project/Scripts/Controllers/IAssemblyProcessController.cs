using System;
using _project.Scripts.Controllers;

public interface IAssemblyProcessController
{
    public Action<int,AssemblyStep> OnDisplayStep { get; set; }
    public Action OnShowPanel { get; set; }
    int TotalNumberOfSteps { get; }

    void ValidateStep();
    void GoToPreviousStep();
    void OpenDictationPanelForRemarkReporting();
    void OpenDictationPanelForIssueReporting();
}