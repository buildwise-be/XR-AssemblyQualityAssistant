using _project.Scripts.Controllers;
using UnityEngine;

namespace _project.Scripts.Views
{
    public class AssemblyReportView : MonoBehaviour
    {
        private IAssemblyProcessController _assemblyProcessController;

        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _stepInfoPrefab;
        [SerializeField] private Transform _container;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _assemblyProcessController = FindFirstObjectByType<AssemblyProcessController>();
            _assemblyProcessController.OnEndProcessEvent += DisplayPanel;
        }

        private void DisplayPanel()
        {
            var stepDataArray = _assemblyProcessController.GetAssemblyProcessData();
            foreach (var stepData in stepDataArray)
            {
                var stepInfoInstance = Instantiate(_stepInfoPrefab, _container);
                stepInfoInstance.GetComponent<IStepInfoView>().SetStepData(stepData);
            }
            _panel.SetActive(true);
        }
    }
}