using System;
using _project.Scripts.Controllers.DTO;

namespace _project.Scripts.Controllers
{
    public interface IDictationPanelController
    {
        void StopDictationProcess();
        Action OnOpenPanel { get; set; }
        Action OnRefreshPanel { get; set; }
        Action OnClosePanel { get; set; }
        void ProcessDictationData(string dictations);
        IRemarksCollection GetSavedRemarks();
    }
}