using UnityEngine;

namespace _project.Scripts.Controllers
{
    public interface IModuleSelectorController
    {
        void OnModuleRayHover(GameObject sGameObject);
        void OnModuleRayExit(GameObject sGameObject);
        void OnModuleSelection();
        AssemblyProjectScriptableObject GetData();
    }
}