using System;
using System.Collections.Generic;

public interface IDictationPanelController
{
    void CloseDictation(string message);
    Action OnOpenPanel { get; set; }
    void ProcessDictationData(List<string> dictations);
}