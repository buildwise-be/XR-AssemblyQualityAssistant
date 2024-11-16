using System;

public interface IAssemblyProcessController
{
    public Action<int,AssemblyStep> OnDisplayStep { get; set; }
    public Action OnShowPanel { get; set; }

    void ValidateStep();
    void GoToPreviousStep();
    void OpenDictationPanelForRemarkReporting();
    void OpenDictationPanelForIssueReporting();
}