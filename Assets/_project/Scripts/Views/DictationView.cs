using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _project.Scripts.Controllers;
using _project.Scripts.Views;
using MixedReality.Toolkit.Examples.Demos;
using TMPro;
using UnityEngine;

public class DictationView : MonoBehaviour
{
    
    private IDictationPanelController _dictationController;
    private DictationHandler _dictationHandler;
    private List<string> _dictations = new();
    private static bool _exitButtonHasBeenPressed;
    [SerializeField] private GameObject _remarksButtonPrefab;
    [SerializeField] private Transform _remarksButtonContainer;
    [SerializeField] private TMP_Text _dictationTextField;
    [SerializeField] private RecordingButtonView _recordingButton;
    private bool _isRecording;
    private StringBuilder _stringBuilder;
    [SerializeField] private GameObject _panel;

    private void Awake()
    {
        _dictationHandler = FindFirstObjectByType<DictationHandler>();
        _dictationController = FindFirstObjectByType<DictationPanelController>();

        _stringBuilder = new StringBuilder();
    }

    private void Start()
    {
        _dictationController.OnOpenPanel += HandleDictationProcess;
        _dictationController.OnRefreshPanel += RefreshView;
    }

    private void RefreshView()
    {
        ClearDictations();
        ClearRemarks();
        LoadPreviousRemarks();
    }

    private void ClearRemarks()
    {
        for (var i = _remarksButtonContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(_remarksButtonContainer.GetChild(i).gameObject);
        }
    }

    private void OnEnable()
    {
        _dictationHandler.OnSpeechRecognized.AddListener(SaveSpeech);
    }

    private void OnDisable()
    {
        _dictationHandler.OnSpeechRecognized.RemoveListener(SaveSpeech);
    }

    public void OnSaveButtonClicked()
    {
        _dictationController.ProcessDictationData(_stringBuilder.ToString());
        _stringBuilder.Clear();
    }
    public void OnExitButtonClicked()
    {
        _exitButtonHasBeenPressed = true;
    }

    

    public void OnStartRecordingButtonClick()
    {
        if (_isRecording) StopRecording();
        else StartRecording();

        
    }

    private void StopRecording()
    {
        _isRecording = false;
        _dictationHandler.StopRecognition();
        _recordingButton.ResetElement();
        #if UNITY_EDITOR
        SaveSpeech("This is a debug text");
        #endif
    }

    private void StartRecording()
    {
        _dictationTextField.SetText(string.Empty);
        _isRecording = true;
        StartCoroutine(OnStartRecordingCoroutine());
    }

    private IEnumerator OnStartRecordingCoroutine()
    {
        yield return _recordingButton.StartRecordingCoroutine();
        #if UNITY_EDITOR
        Debug.Log("Start Recording");
        #else
        _dictationHandler.StartRecognition();
        #endif
        
    }

    private void SaveSpeech(string arg0)
    {
#if UNITY_EDITOR
        arg0 = $"#{_dictations.Count + 1}: {arg0}";
#endif
        _dictations.Add(arg0);
        _dictationTextField.SetText(String.Empty);
        _stringBuilder.Clear();
        foreach (var dictation in _dictations)
        {
            _stringBuilder.Append(dictation + "\n\n");
        }
        _dictationTextField.SetText(_stringBuilder.ToString());
    }

    public void ClearDictations()
    {
        _dictations.Clear();
    }

    private void OnDestroy()
    {
        _dictationController.OnOpenPanel -= HandleDictationProcess;
        _dictationHandler.OnSpeechRecognized.RemoveListener(SaveSpeech);
    }

    private async void HandleDictationProcess()
    {
        _panel.SetActive(true);
        ClearDictations();
        ClearRemarks();
        LoadPreviousRemarks();
        _dictations = new List<string>();
        _exitButtonHasBeenPressed = false;
        await ProcessDictation();
        if (_stringBuilder.Length > 0)
        {
            Debug.Log("String Builder contains data");
        }
        //_dictationController.ProcessDictationData(_stringBuilder.ToString());
        _panel.SetActive(false);
    }

    private void LoadPreviousRemarks()
    {
        var remarksCollection = _dictationController.GetSavedRemarks();
        var previousMessages = remarksCollection.GetRemarkMessages();
        for(var i =0;i< previousMessages.Length;i++)
        {
            var remarkButtonInstance = Instantiate(_remarksButtonPrefab, _remarksButtonContainer);
            remarkButtonInstance.GetComponent<IRemarkButtonView>().SetText(previousMessages[i], _dictationTextField);
            remarkButtonInstance.GetComponent<IRemarkButtonView>().SetTitle($"Remark #{i}");
        }
    }

    private async Task ProcessDictation()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        var buttonTask = SelectButtonAsync(cancellationToken);
        await Task.WhenAll(buttonTask);
    }

    private static async Task SelectButtonAsync(CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested == false)
        {
            if (_exitButtonHasBeenPressed) return;
            await Task.Yield();
        }
    }
}