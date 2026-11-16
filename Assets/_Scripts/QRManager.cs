using Meta.XR.MRUtilityKit;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class QRManager : MonoBehaviour
{
    [FormerlySerializedAs("_mrukInstance")] [SerializeField]
    private MRUK mrukInstance;

    [FormerlySerializedAs("_qrCodeSpawnPrefab")] [SerializeField]
    private GameObject qrCodeSpawnPrefab;

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
    }

    private void OnTrackableAdded(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
        {
            return;
        }
        var instance = Instantiate(qrCodeSpawnPrefab, trackable.transform);

        Debug.Log("<<< QRCode detected! >>>>" + nameof(OnTrackableAdded));

        Debug.Log($"<<< QRCode ID: {trackable.name}, Position: {trackable.transform.position}, Rotation: {trackable.transform.rotation.eulerAngles} >>>");

        Debug.Log("<<< 2D Bounding Box points: " + trackable.PlaneRect + ">>>");

        Debug.Log("<<< 2D Polygon points: " + trackable.PlaneBoundary2D + ">>>");

        if (trackable.MarkerPayloadString is { } str)
        {
            Debug.Log("<<< Payload is a string: " + str + ">>>");
        }
        else if (trackable.MarkerPayloadBytes is { } bytes)
        {
            Debug.Log($"Binary(data=[{string.Join(" ", bytes.Take(16).Select(b => $"{b:x02}"))}{(bytes.Length > 16 ? " ..." : "")}], length={bytes.Length})");
        }
        else
        {
            Debug.Log("<<<< No payload >>>>");
        }
    }

    public void OnTrackableRemoved(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
        {
            return;
        }

        Debug.Log("<<< QRCode removed >>>");
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

