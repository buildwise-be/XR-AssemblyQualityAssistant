using System;
using _project.Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.Views
{
    public class AssemblyReportView : MonoBehaviour
    {
        private IAssemblyProcessController _assemblyProcessController;
        [Header("UI Elements")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _totalDurationText;
        [Header("Prefabs and Containers")]
    
        [SerializeField] private GameObject _stepInfoPrefab;
        [SerializeField] private GameObject _endOfProcessPrefab;
        [SerializeField] private Transform _container;
        private RectTransform _containerRectTransform;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _assemblyProcessController = FindFirstObjectByType<AssemblyProcessController>();
            _assemblyProcessController.OnEndProcessEvent += DisplayPanel;
            _containerRectTransform = _container.GetComponent<RectTransform>();
        }

        private void DisplayPanel(bool isPrematureEnd)
        {
            CreateReport();
            if (isPrematureEnd) SignalPrematureEnd();

            _panel.SetActive(true);
        }

        private void SignalPrematureEnd()
        {
            Instantiate(_endOfProcessPrefab, _container);
        }

        private void CreateReport()
        {
            var stepInfoDataArray = _assemblyProcessController.GetAssemblyProcessData();
            var totalDuration = 0f;
            for (var i = 0; i < stepInfoDataArray.Length; i++)
            {
                if (stepInfoDataArray[i].m_nbOfStepSession == 0) continue;
                
                var stepInfoInstance = Instantiate(_stepInfoPrefab, _container);
                var stepInfoView = stepInfoInstance.GetComponent<IStepInfoView>();
                
                stepInfoView.SetStepData(stepInfoDataArray[i]);
                stepInfoView.SetIllustation(i+1,_assemblyProcessController.GetAssemblyStepIllustation(i));
                stepInfoView.SetTitle(i+1,_assemblyProcessController.GetAssemblyStepTitle(i));
                var stepDuration = stepInfoDataArray[i].m_stepDuration;
                totalDuration += stepDuration;
                string durationString = ConvertDuration(stepDuration);
                stepInfoView.SetDuration(durationString);
            }
            _totalDurationText.text = "Total duration: "+ConvertDuration(totalDuration);
        }

        private string ConvertDuration(float stepDuration)
        {
            var durationInMinutes = Mathf.FloorToInt(stepDuration/60);
            var restInSeconds = Mathf.FloorToInt(stepDuration - durationInMinutes * 60);
            
            if (durationInMinutes == 0) return restInSeconds+" sec";
            return durationInMinutes + " min " + restInSeconds + " sec";
        }

        private void Update()
        {
            //TODO: Rework for better performance
            LayoutRebuilder.ForceRebuildLayoutImmediate(_containerRectTransform);
        }

        public void ClosePanel()
        {
            _panel.SetActive(false);
            _assemblyProcessController.CloseAssemblyProcess();
        }
    }
}