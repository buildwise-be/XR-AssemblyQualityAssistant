using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _project.Scripts.Controllers;
using _project.Scripts.Views;
using MixedReality.Toolkit;
using MixedReality.Toolkit.Examples.Demos;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class DictationView : MonoBehaviour
{
    
    private IDictationPanelController _dictationController;
    private DictationHandler _dictationHandler;
    private List<string> _dictations = new();
    private static bool _exitButtonHasBeenPressed;
    [Header("UI Elements Prefabs")]
    [SerializeField] private GameObject _remarksButtonPrefab;
    [SerializeField] private GameObject _pendingCommentPrefab;
    [Header("UI Elements Containers")]
    [SerializeField] private Transform _remarksButtonContainer;
    [SerializeField] private Transform _pendingCommentContainer;
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _dictationTextField;
    [SerializeField] private GameObject _panel;
    [SerializeField] private RecordingButtonView _recordingButton;
    [SerializeField] private StatefulInteractable _saveButton;
    [SerializeField] private StatefulInteractable _deleteButton;
    [SerializeField] private GameObject _remarkIcon;
    [SerializeField] private GameObject _issueIcon;
    [SerializeField] private ToogleXRManipulator _prematureEndOfAssemblyProcessToggle;
    
    private bool _isRecording;
    private StringBuilder _stringBuilder;
    private List<IRemarkButtonView> _listOfRemarks;
    private ScrollViewContentActivator _scrollViewContentActivator;


    private void Awake()
    {
        _dictationHandler = FindFirstObjectByType<DictationHandler>();
        _dictationController = FindFirstObjectByType<DictationPanelController>();

        _stringBuilder = new StringBuilder();
        _listOfRemarks = new List<IRemarkButtonView>();
    }

    private void Start()
    {
        _dictationController.OnOpenPanel += HandleDictationProcess;
        _dictationController.OnRefreshPanel += RefreshView;
        _dictationController.OnClosePanel += ClosePanel;
        _prematureEndOfAssemblyProcessToggle.OnValueChanged+=OntoggleValueChanged;
        
    }

    private void OntoggleValueChanged(bool arg0)
    {
        _dictationController.MustEndAssemblyProcessOnDictationEnd = arg0;
    }

    private void ClosePanel()
    {
        //gameObject.SetActive(false);
        _listOfRemarks.Clear();
        ClearDictations();
        ClearPendingCommentContainer();
    }

    private void RefreshView()
    {
        ClearDictations();
        ClearPendingCommentContainer();
        ClearRemarks();
        LoadPreviousRemarks();
        
    }

    private void ClearRemarks()
    {
        for (var i = _remarksButtonContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(_remarksButtonContainer.GetChild(i).gameObject);
        }
        _listOfRemarks.Clear();
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
        foreach (var kvp in _dictations)
        {
            _stringBuilder.AppendLine(kvp);
            _stringBuilder.AppendLine();
        }
        _dictationController.ProcessDictationData(_stringBuilder.ToString());
        _dictations.Clear();
        _pendingCommentContainer.gameObject.SetActive(false);
        _stringBuilder.Clear();
        ActivateRemarksButtons(true);
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
        SaveSpeech("DEBUG SPEECH - Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                   "Pellentesque convallis lacus et cursus efficitur. Integer sed laoreet turpis.");
#endif
    }

    private void StartRecording()
    {
        _pendingCommentContainer.gameObject.SetActive(false);
        _dictationTextField.SetText(string.Empty);
        ActivateRemarksButtons(false);
        _isRecording = true;
        StartCoroutine(OnStartRecordingCoroutine());
    }

    private void ActivateRemarksButtons(bool value)
    {
        foreach (var kvp in _listOfRemarks)
        {
            kvp.SetInteractable(value);
        }
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
        ClearPendingCommentContainer();
        DisplayPendingComments();
       
        //_dictationTextField.SetText(_stringBuilder.ToString());
    }

    private void HandlePendingCommmentDelete(int obj)
    {
        ClearPendingCommentContainer();
        _dictations.RemoveAt(obj);
        if (_dictations.Count == 0)
        {
            _pendingCommentContainer.gameObject.SetActive(false);
            ActivateRemarksButtons(true);
            return;
        }
        DisplayPendingComments();
    }

    private void DisplayPendingComments()
    {
        foreach (var dictation in _dictations)
        {
            var pendingComment = Instantiate(_pendingCommentPrefab, _pendingCommentContainer);
            var pendingCommentView = pendingComment.GetComponent<PendingComment>();
            pendingCommentView.SetText(dictation);
            pendingCommentView.OnDeleteButtonEventClick += HandlePendingCommmentDelete;
        }
        _saveButton.enabled = _dictations.Count > 0;
        _deleteButton.enabled = _dictations.Count > 0;
        
        _pendingCommentContainer.gameObject.SetActive(true);
    }

    private void ClearPendingCommentContainer()
    {
        if(_pendingCommentContainer.childCount == 0) return;
        for (var i = _pendingCommentContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(_pendingCommentContainer.GetChild(i).gameObject);
        }
    }

    public void ClearDictations()
    {
        _dictations.Clear();
        _saveButton.enabled = false;
        _deleteButton.enabled = false;
    }

    private void OnDestroy()
    {
        _dictationController.OnOpenPanel -= HandleDictationProcess;
        _dictationHandler.OnSpeechRecognized.RemoveListener(SaveSpeech);
    }

    private async void HandleDictationProcess(bool isInIssueMode)
    {
        _prematureEndOfAssemblyProcessToggle.SetToggleValue(false);
        _panel.SetActive(true);
        _dictationTextField.SetText(string.Empty);
        if (isInIssueMode) SetIssueMode();
        else SetRemarkMode();
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
        _panel.SetActive(false);
        _dictationController.StopDictationProcess();
    }

    private void SetRemarkMode()
    {
        _issueIcon.SetActive(false);
        _remarkIcon.SetActive(true);
        _prematureEndOfAssemblyProcessToggle.gameObject.SetActive(false);
    }

    private void SetIssueMode()
    {
        _issueIcon.SetActive(true);
        _remarkIcon.SetActive(false);
        _prematureEndOfAssemblyProcessToggle.gameObject.SetActive(true);
    }

    private void LoadPreviousRemarks()
    {
        _listOfRemarks.Clear();
        var remarksCollection = _dictationController.GetSavedRemarks();
        var previousMessages = remarksCollection.GetRemarkMessages();
        var remarkType = remarksCollection.GetRemarkType();
        GameObject[] listOfGameObject = new GameObject[remarkType.Length]; 
        for(var i =0;i< remarkType.Length;i++)
        {
            var remarkButtonInstance = Instantiate(_remarksButtonPrefab, _remarksButtonContainer);
            listOfGameObject[i] = remarkButtonInstance;
            var remarkButtonView = remarkButtonInstance.GetComponent<IRemarkButtonView>();
            remarkButtonView.SetText(previousMessages[i], _dictationTextField);
            remarkButtonView.SetIcon(remarkType[i]);
            remarkButtonView.SetTitle($"Remark #{i+1}");
            _listOfRemarks.Add(remarkButtonView);
        }
        _scrollViewContentActivator.SetContentList(listOfGameObject);
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