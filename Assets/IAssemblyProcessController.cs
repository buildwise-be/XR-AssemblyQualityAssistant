using System;

internal interface IAssemblyProcessController
{
    public Action<AssemblyStep> OnDisplayStep { get; set; }

    void ValidateStep();
}