using System;
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

        private async Task Start()
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

        public void StartTracking()
        {
            QRCodeTrackingService.Enable();
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
            markerHolder.localScale = Vector3.one * lastMessage.PhysicalSideLength;
            markerDisplay.SetActive(true);
            PositionSet?.Invoke(this, pose);
            audioSource.Play();
        }

        public EventHandler<Pose> PositionSet;
    }
}