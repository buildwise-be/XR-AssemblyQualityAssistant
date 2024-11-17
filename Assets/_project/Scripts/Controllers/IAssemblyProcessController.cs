using System;
using UnityEngine;

namespace _project.Scripts.Controllers
{
    public interface IAssemblyProcessController
    {
        public Action<int,AssemblyStep> OnDisplayStep { get; set; }
        public Sprite GetAssemblyStepIllustation(int index);
        public Action OnShowPanel { get; set; }
        int TotalNumberOfSteps { get; }
        Action OnEndProcessEvent { get; set; }

        void ValidateStep();
        void GoToPreviousStep();
        void OpenDictationPanelForRemarkReporting();
        void OpenDictationPanelForIssueReporting();
        AssemblyProcessDataDto[] GetAssemblyProcessData();
        string GetAssemblyStepTitle(int i);
    }
}