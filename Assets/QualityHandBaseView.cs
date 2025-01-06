using System;
using _project.Scripts.Controllers;
using UnityEngine;

public class QualityHandBaseView : MonoBehaviour
{
    private MainManager _manager;

    
    private void Start()
    {
        _manager = FindFirstObjectByType<MainManager>();
    }

    public void OnQualityButtonClicked()
    {
        _manager.BeginQualityMonitoring();
    }
}
