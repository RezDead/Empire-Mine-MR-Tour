using Meta.XR.MRUtilityKit;
using UnityEngine;
using System.Collections.Generic;
using _UI;

namespace _Scripts
{
    public class QRManager : MonoBehaviour
    {
        [SerializeField]
        private MRUK mrukInstance;

        [SerializeField]
        private GameObject qrCodeSpawnPrefab;
        
        [SerializeField]
        private GameObject test;
        
        private static Dictionary<MRUKTrackable, GameObject> _qrCodePrefabs = new Dictionary<MRUKTrackable, GameObject>();

        private static QRManager _instance;

        public static bool trackingEnabled
        {
            get => _instance && _instance.mrukInstance && _instance.mrukInstance.SceneSettings.TrackerConfiguration.QRCodeTrackingEnabled;
            set
            {
                if (!_instance || !_instance.mrukInstance)
                {
                    return;
                }
                var config = _instance.mrukInstance.SceneSettings.TrackerConfiguration;
                config.QRCodeTrackingEnabled = value;
                _instance.mrukInstance.SceneSettings.TrackerConfiguration = config;
            }
        }

        private void OnEnable()
        {
            _instance = this;

            if (!mrukInstance)
            {
                Debug.Log($"{nameof(QRManager)} requires an MRUK object in the scene!");
                return;
            }

            mrukInstance.SceneSettings.TrackableAdded.AddListener(OnTrackableAdded);
            mrukInstance.SceneSettings.TrackableRemoved.AddListener(OnTrackableRemoved);

            if (test && test.TryGetComponent<StatusDisplay>(out var component))
            {
                component.SetTitle(trackingEnabled ? "Tracking Enabled" : "Tracking Disabled");
                Debug.Log($"Tracking Enabled: {trackingEnabled}");
            }
            else
            {
                Debug.Log($"{nameof(QRManager)} requires an StatusDisplay object in the scene!");
            }
        }

        private void OnTrackableAdded(MRUKTrackable trackable)
        {
            if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            {
                return;
            }
            var instance = Instantiate(qrCodeSpawnPrefab, trackable.transform);
            
            _qrCodePrefabs.Add(trackable, instance);
        
            Debug.Log($"QR code Found! Payload: {trackable.MarkerPayloadString} added");
            
            InitInfoDisplay(trackable, instance);
        }

        private static void InitInfoDisplay(MRUKTrackable trackable, GameObject instance)
        {
            if (!instance)
            {
                Debug.LogWarning("QR code prefab not found!");
                return;
            }

            var infoDisplay = instance.GetComponentInChildren<InformationDisplay>(true);

            if (!infoDisplay)
            {
                Debug.LogWarning("QR code prefab does not have an InformationDisplay script!");
                return;
            }
            
            infoDisplay.SetTitle(trackable.MarkerPayloadString);
            infoDisplay.SetBody(trackable.transform.ToString());
        }

        private static void OnTrackableRemoved(MRUKTrackable trackable)
        {
            if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            {
                return;
            }

            _qrCodePrefabs.Remove(trackable);

            Debug.Log($"QR: {trackable.MarkerPayloadString} removed");
        }

        private void OnDisable()
        {
            if (mrukInstance)
            {
                mrukInstance.SceneSettings.TrackableAdded.RemoveListener(OnTrackableAdded);
                mrukInstance.SceneSettings.TrackableRemoved.RemoveListener(OnTrackableRemoved);
            }
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}

