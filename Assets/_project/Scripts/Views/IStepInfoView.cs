using _project.Scripts.Controllers;
using UnityEngine;

namespace _project.Scripts.Views   
{
    internal interface IStepInfoView
    {
        void SetStepData(AssemblyProcessDataDto stepData);
        void SetIllustation(int i, Sprite illustrationSprite);
        void SetTitle(int assemblyStepTitle, string getAssemblyStepTitle);
    }
}