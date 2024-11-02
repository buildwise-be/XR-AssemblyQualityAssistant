using System;

public interface IAssemblyProcessController
{
    public Action<AssemblyStep> OnDisplayStep { get; set; }

    void ValidateStep();
    void OpenDictationPanel();
}