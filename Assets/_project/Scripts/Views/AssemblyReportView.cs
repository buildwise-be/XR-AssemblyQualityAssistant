using System;
using _project.Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;

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
            var stepInfoDataArray = _assemblyProcessController.GetAssemblyProcessData();

            for (var i = 0; i < stepInfoDataArray.Length; i++)
            {
 
                var stepInfoInstance = Instantiate(_stepInfoPrefab, _container);
                var stepInfoView = stepInfoInstance.GetComponent<IStepInfoView>();
                
                stepInfoView.SetStepData(stepInfoDataArray[i]);
                stepInfoView.SetIllustation(i+1,_assemblyProcessController.GetAssemblyStepIllustation(i));
                stepInfoView.SetTitle(i+1,_assemblyProcessController.GetAssemblyStepTitle(i));
                var durationInMinutes = Mathf.FloorToInt(stepInfoDataArray[i].m_stepDuration / 60);
                stepInfoView.SetDuration(durationInMinutes+" minutes");
            }

            _panel.SetActive(true);
        }

        private void Update()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_container.GetComponent<RectTransform>());
        }
    }
}