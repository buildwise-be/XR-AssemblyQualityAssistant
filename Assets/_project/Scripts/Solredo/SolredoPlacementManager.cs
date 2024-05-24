using UnityEngine;
using MixedReality.Toolkit.Input;
using UnityEngine.InputSystem;
using MixedReality.Toolkit.Subsystems;
using MixedReality.Toolkit;
using UnityEngine.Events;
using MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using UnityEngine.XR.ARFoundation;
using MRTKExtensions.QRCodes;

public class SolredoPlacementManager : MonoBehaviour
{
    // QR
    [SerializeField]
    private QRTrackerController trackerController;
    private bool _allowQRDetection = false;

    [SerializeField] private ArticulatedHandController _rightHandController;
    [SerializeField] private ArticulatedHandController _leftHandController;
    [SerializeField] private GameObject _miniatureModelPrefab;
    private GameObject _miniatureModel;
    [HideInInspector]
    public UnityEvent<GameObject> OnMiniatureInstantiated;
    public UnityEvent<int> OnModuleSelected;
    public UnityEvent OnModulePlaced;

    //private MRTKBaseInteractable _hoveredPlane;
    [HideInInspector]
    public GameObject ChosenModule;
    private GameObject _chosenModuleInstance;

    private MRTKRayInteractor _rightRayInteractor;
    private LineRenderer _rightLineRenderer;
    private Coroutine _showModuleCoroutine;

    [SerializeField]
    private UIManager _UIManager;

    private IHandsAggregatorSubsystem handsAggregatorSubsystem;

    private bool _isMiniatureCreationAuthorized = false;
    public bool IsMiniatureCreationAuthorized
    {
        get
        {
            return _isMiniatureCreationAuthorized;
        }
        set
        {
            _isMiniatureCreationAuthorized = value;
        }
    }

    void Start()
    {
        _rightHandController.selectAction.action.performed += OnPinchRight;
        _leftHandController.selectAction.action.performed += OnPinchLeft;
        handsAggregatorSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<IHandsAggregatorSubsystem>();

        //Get the MRTK Ray Interactors
        _rightRayInteractor = _rightHandController.GetComponentInChildren<MRTKRayInteractor>();
        _rightLineRenderer = _rightRayInteractor.GetComponentInChildren<LineRenderer>(true);

        trackerController.PositionSet += PoseFound;
    }

    public void AllowQRDetection(bool on)
    {
        _allowQRDetection = on;
    }

    private void PoseFound(object sender, Pose pose)
    {
        if (_allowQRDetection)
        {
            if (_chosenModuleInstance == null)
            {
                _chosenModuleInstance = Instantiate(ChosenModule);
            }
            //_UIManager.ShowInfoDialog("Pose found", $"module placé en {pose.position}");
            _chosenModuleInstance.transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
        /*
        var childObj = transform.GetChild(0);
        childObj.SetPositionAndRotation(pose.position, pose.rotation);
        childObj.gameObject.SetActive(true);
        */
    }

    #region PinchDetection
    /// <summary>
    /// Gets the pinch world position of the hand controller
    /// </summary>
    /// <param name="handController"></param>
    /// <returns></returns>
    private Vector3 GetPinchPosition(ArticulatedHandController handController)
    {
        return handsAggregatorSubsystem.TryGetPinchingPoint(handController.HandNode,
                                                            out var jointPose)
            ? jointPose.Position
            : Vector3.zero;
    }

    private void OnPinchLeft(InputAction.CallbackContext context)
    {
        if (_isMiniatureCreationAuthorized)
        {
            Vector3 pinchPos = GetPinchPosition(_rightHandController);
            Vector3 pinchPosForward = pinchPos + 2*Camera.main.transform.forward;
            PlaceMiniatureModel(pinchPosForward);
        }
        _isMiniatureCreationAuthorized = false;
    }

    private void OnPinchRight(InputAction.CallbackContext context)
    {
        if (_isMiniatureCreationAuthorized)
        {
            Vector3 pinchPos = GetPinchPosition(_rightHandController);
            Vector3 pinchPosForward = pinchPos + 2*Camera.main.transform.forward;
            PlaceMiniatureModel(pinchPosForward);
        }
        _isMiniatureCreationAuthorized = false;
    }
    #endregion

    #region Miniature
    public void AllowMiniatureCreation()
    {
        _isMiniatureCreationAuthorized = true;
    }

    private void PlaceMiniatureModel(Vector3 handPosition)
    {
        if (_miniatureModel == null)
        {
            _miniatureModel = Instantiate(_miniatureModelPrefab, handPosition, Quaternion.identity);
            OnMiniatureInstantiated?.Invoke(_miniatureModel);
        }
    }

    public void LockMiniatureOnOff()
    {
        bool isCurrentlyLocked = _miniatureModel.GetComponent<ObjectManipulator>().AllowedInteractionTypes == InteractionFlags.None;
        if (isCurrentlyLocked)
        {
            _miniatureModel.GetComponent<ObjectManipulator>().AllowedInteractionTypes = InteractionFlags.Near | InteractionFlags.Ray;
        }
        else
        {
            _miniatureModel.GetComponent<ObjectManipulator>().AllowedInteractionTypes = InteractionFlags.None;
        }
        _miniatureModel.GetComponent<BoxCollider>().enabled = isCurrentlyLocked;
    }
    #endregion


    public void AddShowModulesOnRayEndEvent(ARPlane plane)
    {
        MRTKBaseInteractable _hoveredPlane = plane.GetComponent<MRTKBaseInteractable>();
        _hoveredPlane.IsRayHovered.OnEntered.AddListener(ShowModuleOnRayEnd);
        _hoveredPlane.IsRayHovered.OnExited.AddListener(HideModuleOnRayEnd);
        _hoveredPlane.IsRaySelected.OnEntered.AddListener(PlaceModuleOnRayEnd);
    }

    private void PlaceModuleOnRayEnd(float arg0)
    {
        Vector3 moduleFinalPosition = _chosenModuleInstance.transform.position;
        if (_showModuleCoroutine != null)
        {
            StopCoroutine(_showModuleCoroutine);
        }
        foreach (ARPlane p in FindObjectsOfType<ARPlane>())
        {
            if (p.TryGetComponent<MRTKBaseInteractable>(out MRTKBaseInteractable i))
            {
                i.IsRayHovered.OnEntered.RemoveAllListeners();
                i.IsRayHovered.OnExited.RemoveAllListeners();
                i.IsRaySelected.OnEntered.RemoveAllListeners();
            }
            p.gameObject.SetActive(false);
        }
        
        _chosenModuleInstance.transform.position = moduleFinalPosition;
        _chosenModuleInstance.GetComponent<BoxCollider>().enabled = true;
        OnModulePlaced?.Invoke();
    }

    private void HideModuleOnRayEnd(float arg0)
    {
        //_chosenModuleInstance.SetActive(false);
    }

    private void ShowModuleOnRayEnd(float time)
    {
        if (_chosenModuleInstance == null)
        {
            _chosenModuleInstance = Instantiate(ChosenModule);
        }
        if (_showModuleCoroutine != null)
        {
            StopCoroutine(_showModuleCoroutine);
        }
        _showModuleCoroutine = StartCoroutine(ShowModuleOnRayEndCoroutine());
    }

    private IEnumerator ShowModuleOnRayEndCoroutine()
    {
        while (true)
        {
            if (_rightLineRenderer.positionCount > 0)
            {
                var rayEndPos = _rightLineRenderer.GetPosition(_rightLineRenderer.positionCount - 1);
                _chosenModuleInstance.transform.position = rayEndPos;
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        _rightHandController.selectAction.action.performed -= OnPinchRight;
        _leftHandController.selectAction.action.performed -= OnPinchRight;
    }

    #region SceneUnderstanding
    /*
    /// <summary>
    /// Gets an immutable scene. Call this when you want to get the current scene and when the room has been scanned.
    /// </summary>
    /// <returns></returns>
    private async Task InitSceneAsync()
    {
        if (Application.isEditor)
        {
            Debug.LogError("InitSceneAsync: Running in editor while quering scene from a device is not supported.\n" +
                           "To run on editor disable the 'QuerySceneFromDevice' Flag in the SceneUnderstandingManager Component");
            return;
        }
        if (!SceneObserver.IsSupported())
        {
            Debug.LogError("Scene Understanding is not supported on this device.");
        }

        // This call should grant the access we need.
        SceneObserverAccessStatus access = await SceneObserver.RequestAccessAsync();
        if (access != SceneObserverAccessStatus.Allowed)
        {
            Debug.LogError("SceneUnderstandingManager.Start: Access to Scene Understanding has been denied.\n" +
                           "Reason: " + access);
            return;
        }

        _sceneQuerySettings = new SceneQuerySettings()
        {
            EnableWorldMesh = true,                                         // Request a static version of the spatial mapping
            EnableSceneObjectMeshes = true,
            EnableSceneObjectQuads = true,
            EnableOnlyObservedSceneObjects = false,                         // Do not explicitly turn off quad inference
            RequestedMeshLevelOfDetail = SceneMeshLevelOfDetail.Fine
        };

        // Initialize a new Scene
        scene = await SceneObserver.ComputeAsync(_sceneQuerySettings, 10.0f);
    }

    private async Task UpdateScene()
    {
        scene = await SceneObserver.ComputeAsync(_sceneQuerySettings, 10.0f, scene); // Update the scene, and keep all object's IDs the same by passing the previous scene
    }

    private SceneObject[] GetFloorsInScene()
    {
        return scene.SceneObjects.Where<SceneObject>(x => x.Kind == SceneObjectKind.Floor).ToArray(); // Maybe useful to only raycast against floors
        /*
        foreach (var floor in scene.SceneObjects)
        {
            if (floor.Kind == SceneObjectKind.Floor)
            {
                Debug.Log("Floor found");
            }
        }
        */
    //}
    #endregion
}
