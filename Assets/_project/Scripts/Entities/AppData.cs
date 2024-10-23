using UnityEngine;

namespace _project.Scripts.Entities
{
    [CreateAssetMenu(menuName ="BuildWize/App Data")]
    internal class AppData : ScriptableObject
    {
        public AssemblyProjectScriptableObject project;
    }
}