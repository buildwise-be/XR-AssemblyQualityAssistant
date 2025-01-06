using UnityEngine;
using MixedReality.Toolkit.Input;
using UnityEngine.InputSystem;
using MixedReality.Toolkit.Subsystems;
using MixedReality.Toolkit;
using UnityEngine.Events;
using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.UX;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private ArticulatedHandController _rightHandController;
    [SerializeField] private ArticulatedHandController _leftHandController;
    [SerializeField] private GameObject _ConcreteSlabPrefab;
    [SerializeField] private float _positionFineTuneRange = 0.2f;
    [SerializeField] private float _rotationFineTuneRange = 0.2f;
    private GameObject _ConcreteSlab;
    [HideInInspector]
    public UnityEvent<GameObject> OnConcreteSlabInstantiated;
    
    public float _translateSpeed = 0.1f;
    private float _previousTranslateSpeed;
    private float _slabCreationDistance = 3.0f;

    //private SceneQuerySettings _sceneQuerySettings;
    //private Scene scene;
    private IHandsAggregatorSubsystem handsAggregatorSubsystem;

    private bool _isSlabCreationAuthorized = false;
    public GameObject Prefab;
    private bool _isRunning;

    public bool IsSlabCreationAuthorized 
    { 
        get
        {
            return _isSlabCreationAuthorized;
        }
        set
        {
            _isSlabCreationAuthorized = value;
        }
    }

    void Start()
    {
        
    }

    public void Initialize()
    {
        _rightHandController.selectAction.action.performed += OnPinchRight;
        _leftHandController.selectAction.action.performed += OnPinchLeft;
        handsAggregatorSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<IHandsAggregatorSubsystem>();

        MRTKRayInteractor rightRay = _rightHandController.GetComponentInChildren<MRTKRayInteractor>();
        rightRay.translateSpeed = _translateSpeed;
        _previousTranslateSpeed = _translateSpeed;
        _isRunning = true;
    }

    private void Update()
    {
        if (_isRunning == false) return;
        if (_previousTranslateSpeed != _translateSpeed)
        {
            MRTKRayInteractor rightRay = _rightHandController.GetComponentInChildren<MRTKRayInteractor>();
            rightRay.translateSpeed = _translateSpeed;
            _previousTranslateSpeed = _translateSpeed;
        }
    }

    private Vector3 GetPinchPosition(ArticulatedHandController handController)
    {
        return handsAggregatorSubsystem.TryGetPinchingPoint(handController.HandNode,
                                                            out var jointPose)
            ? jointPose.Position
            : Vector3.zero;
    }

    private void OnPinchLeft(InputAction.CallbackContext context)
    {
        if (_isSlabCreationAuthorized)
        {
            Vector3 pinchPos = GetPinchPosition(_rightHandController);
            Vector3 pinchPosForward = pinchPos + _slabCreationDistance * Camera.main.transform.forward;
            PlaceConcreteSlab(pinchPosForward);
        }
        _isSlabCreationAuthorized = false;
    }

    private void OnPinchRight(InputAction.CallbackContext context)
    { 
        if (_isSlabCreationAuthorized)
        {
            Vector3 pinchPos = GetPinchPosition(_rightHandController);
            Vector3 pinchPosForward = pinchPos + _slabCreationDistance * Camera.main.transform.forward;
            PlaceConcreteSlab(pinchPosForward);
        }
        _isSlabCreationAuthorized = false;
    }

    public void PlaceConcreteSlab(Vector3 placePosition)
    {
        if (_ConcreteSlab == null)
        {
            _ConcreteSlab = Instantiate(Prefab, placePosition, Quaternion.identity);
            OnConcreteSlabInstantiated?.Invoke(_ConcreteSlab);
        }
    }

    public void LockSlabPosition()
    {
        _ConcreteSlab.GetComponent<ObjectManipulator>().AllowedInteractionTypes = InteractionFlags.None;
        _ConcreteSlab.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
    }

    public void LockSlabPositionOnOff()
    {
        bool isCurrentlyLocked = _ConcreteSlab.GetComponent<ObjectManipulator>().AllowedInteractionTypes == InteractionFlags.None;
        if (isCurrentlyLocked)
        {
            _ConcreteSlab.GetComponent<ObjectManipulator>().AllowedInteractionTypes = InteractionFlags.Near | InteractionFlags.Ray;
        }
        else
        {
            _ConcreteSlab.GetComponent<ObjectManipulator>().AllowedInteractionTypes = InteractionFlags.None;
        }
        _ConcreteSlab.GetComponentInChildren<Canvas>(true).gameObject.SetActive(!isCurrentlyLocked);
    }
    
    public bool IsSlabLocked => _ConcreteSlab.GetComponent<ObjectManipulator>().AllowedInteractionTypes == InteractionFlags.None;

    public void FineTuneVerticalSlabPosition(SliderEventData sliderEventData)
    {
        float old = sliderEventData.OldValue;
        float current = sliderEventData.NewValue;
        float deltaPos = (current - old) * _positionFineTuneRange;
        _ConcreteSlab.transform.localPosition += new Vector3(0, deltaPos, 0);
    }

    public void FineTuneHorizontalSlabPositionX(SliderEventData sliderEventData)
    {
        float old = sliderEventData.OldValue;
        float current = sliderEventData.NewValue;
        float deltaPos = (current - old) * _positionFineTuneRange;
        _ConcreteSlab.transform.localPosition += new Vector3(deltaPos, 0, 0);
    }

    public void FineTuneHorizontalSlabPositionZ(SliderEventData sliderEventData)
    {
        float old = sliderEventData.OldValue;
        float current = sliderEventData.NewValue;
        float deltaPos = (current - old) * _positionFineTuneRange;
        _ConcreteSlab.transform.localPosition += new Vector3(0, 0, deltaPos);
    }

    public void FineTuneSlabRotation(SliderEventData sliderEventData)
    {
        float old = sliderEventData.OldValue;
        float current = sliderEventData.NewValue;
        float deltaRot = (current - old) * _rotationFineTuneRange;

        Debug.Log($"Rotating by {deltaRot} degrees.");
        Quaternion rotation = Quaternion.Euler(0, deltaRot, 0);
        _ConcreteSlab.transform.rotation *= rotation;
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
