using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MixedReality.Toolkit.Examples.Demos;
using TMPro;
using UnityEngine;

public class DictationView : MonoBehaviour
{
    private IDictationPanelController _dictationController;
    private DictationHandler _dictationHandler;
    private List<string> _dictations;
    private static bool _exitButtonHasBeenPressed;
    [SerializeField] private TMP_Text _dictationTextField;
    [SerializeField] private RecordingButtonView _recordingButton;
    private bool _isRecording;

    private void Awake()
    {
        _dictationHandler = FindFirstObjectByType<DictationHandler>();
        _dictationController = FindFirstObjectByType<DictationPanelController>();
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
        _dictations.Add(arg0);
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
        _dictations = new List<string>();
        _exitButtonHasBeenPressed = false;
        await ProcessDictation();
        _dictationController.ProcessDictationData(_dictations);
        gameObject.SetActive(false);
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