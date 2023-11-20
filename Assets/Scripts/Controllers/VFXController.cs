using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using static UnityEngine.XR.ARSubsystems.XRCpuImage;

// - camera feed = reflection probe
// - light direction
// - fog color + intensity

public class VFXController : MonoBehaviour
{
    //[SerializeField]
    //private RawImage _debugCameraFeed;
    //[SerializeField]
    //private RawImage _debugCameraBackground;

    [SerializeField]
    private AREarthManager _earthManager;
    [SerializeField]
    private ARCameraManager _cameraManager;
    //[SerializeField]
    //private AROcclusionManager _occlusionManager;
    [SerializeField]
    private ARCoreCustomCameraBackground _customCameraBackground;

    private VFXModel _vfxModel;

    private void Awake()
    {
        _vfxModel = FindObjectOfType<VFXModel>();
    }

    //private void frameReceivedHandler(ARCameraFrameEventArgs obj)
    //{
    //    if (_customCameraBackground && _customCameraBackground.material.HasProperty("_MainTex"))
    //    {
    //        if (_vfxModel.CurrentCameraFeed == null)
    //            _vfxModel.CurrentCameraFeed = new RenderTexture(Screen.width, Screen.height, 0);

    //        Graphics.Blit(_customCameraBackground.material.GetTexture("_MainTex"), _vfxModel.CurrentCameraFeed);

    //        _debugCameraFeed.texture = _vfxModel.CurrentCameraFeed;

    //        Logger.Add("Camera Feed available");
    //    }
    //}

    //private void frameReceivedHandler(ARCameraFrameEventArgs obj)
    //{
    //    _debugCameraBackground.texture = _cameraBackground.material.mainTexture;
    //}

    private void Update()
    {
        _vfxModel.CurrentTime = DateTime.Now;

        if (_earthManager)
            UpdateEarthPose();

        if (_vfxModel.IsSampleFogColor)
            UpdateSampleFogColor();
    }

    private void UpdateEarthPose()
    {
        if (_earthManager.EarthState == EarthState.Enabled && _earthManager.EarthTrackingState == TrackingState.Tracking)
        {
            _vfxModel.CurrentLatitude = _earthManager.CameraGeospatialPose.Latitude.ToString();
            _vfxModel.CurrentLongitude = _earthManager.CameraGeospatialPose.Longitude.ToString();
            _vfxModel.CurrentHeadingDiff = _earthManager.CameraGeospatialPose.EunRotation;

            //Logger.Add("Heading Diff: " + _vfxModel.CurrentHeadingDiff.eulerAngles.y);

            if (_vfxModel.IsSunDirection)
                UpdateSunDirection();
        }
    }

    private void UpdateSunDirection()
    {
        double alt;
        double azi;
        double lat;
        double.TryParse(_vfxModel.CurrentLatitude, out lat);
        double lon;
        double.TryParse(_vfxModel.CurrentLongitude, out lon);
        SunPosition.CalculateSunPosition(_vfxModel.CurrentTime, lat, lon, out azi, out alt);

        Vector3 angles = new Vector3();
        angles.x = (float)alt * Mathf.Rad2Deg;
        angles.y = (float)azi * Mathf.Rad2Deg;

        _vfxModel.CurrentSunDirection = Quaternion.Euler(angles);
        _vfxModel.UpdateSunDirection();

        //Logger.Add("Sun direction: " + _vfxModel.CurrentSunDirection.eulerAngles);

        //Logger.Add("Current Time: " + _vfxModel.CurrentTime);
        //Logger.Add("Current Lat: " + _vfxModel.CurrentLatitude);
        //Logger.Add("Current Lon: " + _vfxModel.CurrentLongitude);
    }

    public void ToggleSunDirection(bool value)
    {
        _vfxModel.IsSunDirection = value;
        _vfxModel.ToggleSunDirection();
    }

    //public void ToggleOcclusion(bool value)
    //{
    //    _occlusionManager.requestedEnvironmentDepthMode = value ? EnvironmentDepthMode.Medium : EnvironmentDepthMode.Disabled;
    //}

    public void UpdateFogIntensity(float value)
    {
        RenderSettings.fogDensity = value;

        Logger.Add("Fog intensity: " + value);
    }

    public void InitSampleFogColor()
    {
        _vfxModel.IsSampleFogColor = true;
        _vfxModel.IsSampleFogColorStarted = false;

        Logger.Add("Init Sample Fog Color");
    }

    unsafe void SampleCameraFeed(Vector2 position)
    {
        // Attempt to get the latest camera image. If this method succeeds,
        // it acquires a native resource that must be disposed (see below).
        if (!_cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        //Logger.Add(string.Format("Image info:\n\twidth: {0}\n\theight: {1}\n\tplaneCount: {2}\n\ttimestamp: {3}\n\tformat: {4}", image.width, image.height, image.planeCount, image.timestamp, image.format));

        var format = TextureFormat.RGBA32;

        if (_vfxModel.CurrentCameraFeed == null || _vfxModel.CurrentCameraFeed.width != image.width || _vfxModel.CurrentCameraFeed.height != image.height)
            _vfxModel.CurrentCameraFeed = new Texture2D(image.width, image.height, format, false);

        // Convert the image to format, flipping the image across the Y axis.
        var conversionParams = new XRCpuImage.ConversionParams(image, format, XRCpuImage.Transformation.MirrorY);

        // Texture2D allows us write directly to the raw texture data
        // This allows us to do the conversion in-place without making any copies.
        var rawTextureData = _vfxModel.CurrentCameraFeed.GetRawTextureData<byte>();
        try
        {
            image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
        }
        finally
        {
            // We must dispose of the XRCpuImage after we're finished
            // with it to avoid leaking native resources.
            image.Dispose();
        }

        _vfxModel.CurrentCameraFeed.Apply();

        _vfxModel.CurrentFogColor = _vfxModel.CurrentCameraFeed.GetPixel((int)position.x, (int)position.y);

        RenderSettings.fogColor = _vfxModel.CurrentFogColor;

        _vfxModel.UpdateCameraFeed();

        //if(_debugCameraFeed)
        //    _debugCameraFeed.texture = _vfxModel.CurrentCameraFeed;
    }

    private void UpdateSampleFogColor()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!_vfxModel.IsSampleFogColorStarted && touch.phase == TouchPhase.Began)
            {
                _vfxModel.IsSampleFogColorStarted = true;
                SampleCameraFeed(touch.position);
            }
            else if (_vfxModel.IsSampleFogColorStarted && touch.phase == TouchPhase.Moved)
            {
                SampleCameraFeed(touch.position);
            }
            else if (_vfxModel.IsSampleFogColorStarted && touch.phase == TouchPhase.Ended)
            {
                StopSampleFogColor();
            }
        }
    }

    private void StopSampleFogColor()
    {
        _vfxModel.IsSampleFogColor = false;

        _vfxModel.SampleFogColorComplete();

        Logger.Add("Stop sample fog color: " + _vfxModel.CurrentFogColor);
    }
}
