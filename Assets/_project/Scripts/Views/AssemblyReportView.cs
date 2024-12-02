using System;
using _project.Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
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
    
        //[SerializeField] private GameObject _stepInfoPrefab;
        [SerializeField] private GameObject _endOfProcessPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject[] _stepContainer;
        private RectTransform _containerRectTransform;


        private void Awake()
        {
            _stepContainer = new GameObject[_container.childCount];
            for (var i = 0; i < _container.childCount; i++)
            {
                _stepContainer[i] = _container.GetChild(i).gameObject;
            }
        }

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
            transform.SetPositionAndRotation(Camera.main.transform.position + Camera.main.transform.forward * 0.7f, Camera.main.transform.rotation);

        }

        private void SignalPrematureEnd()
        {
            Instantiate(_endOfProcessPrefab, _container);
        }

        private void CreateReport()
        {
            foreach (var VARIABLE in _stepContainer)
            {
                VARIABLE.SetActive(false);
            }
            var stepInfoDataArray = _assemblyProcessController.GetAssemblyProcessData();
            var totalDuration = 0f;
            for (var i = 0; i < stepInfoDataArray.Length; i++)
            {
                if (stepInfoDataArray[i].m_nbOfStepSession == 0) continue;


                var stepInfoInstance = _stepContainer[i];
                var stepInfoView = stepInfoInstance.GetComponent<IStepInfoView>();
                
                stepInfoView.SetStepData(stepInfoDataArray[i]);
                stepInfoView.SetIllustation(i+1,_assemblyProcessController.GetAssemblyStepIllustation(i));
                stepInfoView.SetTitle(i+1,_assemblyProcessController.GetAssemblyStepTitle(i, LocalizationSettings.SelectedLocale.LocaleName));
                var stepDuration = stepInfoDataArray[i].m_stepDuration;
                totalDuration += stepDuration;
                string durationString = ConvertDuration(stepDuration);
                stepInfoView.SetDuration(durationString);
                stepInfoInstance.SetActive(true);
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