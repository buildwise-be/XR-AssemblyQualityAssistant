using System;

public interface IDictationPanelController
{
    void CloseDictation(string message);
    Action OnOpenPanel { get; set; }
}