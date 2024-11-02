using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MixedReality.Toolkit.Examples.Demos;
using TMPro;
using UnityEngine;

public class DictationView : MonoBehaviour
{
    [SerializeField] private GameObject _remarksButtonPrefab;
    private IDictationPanelController _dictationController;
    private DictationHandler _dictationHandler;
    private List<string> _dictations = new();
    private static bool _exitButtonHasBeenPressed;
    [SerializeField] private TMP_Text _dictationTextField;
    [SerializeField] private RecordingButtonView _recordingButton;
    private bool _isRecording;
    private StringBuilder _stringBuilder;

    private void Awake()
    {
        _dictationHandler = FindFirstObjectByType<DictationHandler>();
        _dictationController = FindFirstObjectByType<DictationPanelController>();

        _stringBuilder = new StringBuilder();
    }

    private void Start()
    {
        _dictationController.OnOpenPanel += HandleDictationProcess;
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
        gameObject.SetActive(true);
        LoadPreviousRemarks();
        _dictations = new List<string>();
        _exitButtonHasBeenPressed = false;
        await ProcessDictation();
        _dictationController.ProcessDictationData(_dictations);
        gameObject.SetActive(false);
    }

    private void LoadPreviousRemarks()
    {
        //_dictationController.GetSavedRemarks();
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