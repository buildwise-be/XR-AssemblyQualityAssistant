using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using RealityCollective.ServiceFramework.Services;
using TMPro;
using UnityEngine;

namespace MRTKExtensions.QRCodes
{
    public class QRTrackerController : MonoBehaviour
    {
        [SerializeField]
        private SpatialGraphCoordinateSystemSetter spatialGraphCoordinateSystemSetter;

        [SerializeField]
        private TextMeshProUGUI _debugText;

        [SerializeField]
        private string locationQrValue = string.Empty;

        private Transform markerHolder;
        private AudioSource audioSource;
        private GameObject markerDisplay;
        private QRInfo lastMessage;
        private bool _isProcessingQRCode = false;

        public bool IsTrackingActive { get; private set; } = true;

        private IQRCodeTrackingService qrCodeTrackingService;

        private IQRCodeTrackingService QRCodeTrackingService =>
            qrCodeTrackingService ??= ServiceManager.Instance.GetService<IQRCodeTrackingService>();

        private async void Start()
        {
            try
            {
                // Give service time to start;
                await Task.Delay(250);

                if (!QRCodeTrackingService.IsSupported)
                {
                    if (_debugText != null)
                        _debugText.text += "\nQRCodeTrackingService not supported!";
                    return;
                }

                markerHolder = spatialGraphCoordinateSystemSetter.gameObject.transform;
                markerDisplay = markerHolder.GetChild(0).gameObject;
                markerDisplay.SetActive(false);

                audioSource = markerHolder.gameObject.GetComponent<AudioSource>();

                QRCodeTrackingService.QRCodeFound += ProcessTrackingFound;
                spatialGraphCoordinateSystemSetter.PositionAcquired += SetPosition;
                spatialGraphCoordinateSystemSetter.PositionAcquisitionFailed +=
                    (s, e) => ResetTracking();
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }

        public void StartTracking()
        {
            StartCoroutine(InitializeTrackingCoroutine());
            //QRCodeTrackingService.Enable();
        }

        private IEnumerator InitializeTrackingCoroutine( )
        {
            yield return new WaitForSeconds(1);
            while (ServiceManager.IsActiveAndInitialized==false)
            {
                Debug.Log("InitializeTrackingCoroutine -- WAITING");
                yield return null;
            }

            try
            {
                QRCodeTrackingService.Enable();
            }
            catch(Exception e)
            {
                Debug.Log(e.Data);
            }
            
            Debug.Log("InitializeTrackingCoroutine -- DONE");
        }

            public void ResetTracking()
        {
            if (QRCodeTrackingService.IsInitialized)
            {
                markerDisplay.SetActive(false);
                IsTrackingActive = true;
            }
        }

        private void ProcessTrackingFound(object sender, QRInfo msg)
        {
            if (msg == null || !IsTrackingActive)
            {
                return;
            }
            if (_isProcessingQRCode)
            {
                Debug.Log($"Already processing QR code! Returning.");
                return;
            }
            _isProcessingQRCode = true;
            
            lastMessage = msg;

            if (msg.Data == locationQrValue && Math.Abs((DateTimeOffset.UtcNow - msg.LastDetectedTime.UtcDateTime).TotalMilliseconds) < 200)
            {
                spatialGraphCoordinateSystemSetter.SetLocationIdSize(msg.SpatialGraphNodeId,
                    msg.PhysicalSideLength);
            }
            _isProcessingQRCode = false;
        }

        private void SetPosition(object sender, Pose pose)
        {
            IsTrackingActive = false;
            #if !UNITY_EDITOR
            markerHolder.localScale = Vector3.one * lastMessage.PhysicalSideLength;
            markerDisplay.SetActive(true);
            audioSource.Play();
            #endif
            
            PositionSet?.Invoke(this, pose);
            
        }

        public EventHandler<Pose> PositionSet;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SetPosition(this,new Pose(Vector3.forward,Quaternion.identity));
                enabled = false;
            }
        }
        
    }
}