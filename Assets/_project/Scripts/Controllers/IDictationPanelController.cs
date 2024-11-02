using System;
using _project.Scripts.Controllers.DTO;

namespace _project.Scripts.Controllers
{
    public interface IDictationPanelController
    {
        void CloseDictation(string message);
        Action OnOpenPanel { get; set; }
        Action OnRefreshPanel { get; set; }
        void ProcessDictationData(string dictations);
        IRemarksCollection GetSavedRemarks();
    }
}