using System;
using _project.Scripts.Controllers.DTO;

namespace _project.Scripts.Controllers
{
    public interface IDictationPanelController
    {
        void StopDictationProcess();
        Action<bool> OnOpenPanel { get; set; }
        Action OnRefreshPanel { get; set; }
        Action OnClosePanel { get; set; }
        bool MustEndAssemblyProcessOnDictationEnd { set; }
        void ProcessDictationData(string dictations);
        IRemarksCollection GetSavedRemarks();
    }
}