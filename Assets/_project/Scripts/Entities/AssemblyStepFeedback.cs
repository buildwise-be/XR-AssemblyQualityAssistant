using System;

[Serializable]
public class AssemblyStepFeedback
{
    private readonly int _index;
    public readonly float _startTime;

    public AssemblyStepFeedback(int index, float _startTime)
    {
        _index = index;
        _startTime = _startTime;
    }
}