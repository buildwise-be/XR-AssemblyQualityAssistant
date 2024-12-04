using UnityEngine;

namespace _project.Scripts.Controllers.QualityProject
{
    [CreateAssetMenu(fileName = "New Quality Project ScriptableObject", menuName = "Buildwise/Quality Project ")]
    public class QualityProjectScriptableObject : ScriptableObject
    {
        [ScriptableObjectId] public string m_guid;
        public string projectName;
        public AssemblyStep[] assemblySteps;
        public GameObject Prefab;
    }
}
