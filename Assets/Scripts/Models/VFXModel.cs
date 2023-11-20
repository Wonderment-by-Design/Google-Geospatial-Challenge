using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXModel : MonoBehaviour
{
    public delegate void EventHandler();
    public event EventHandler ToggleSunDirectionHandler;
    public event EventHandler UpdateSunDirectionHandler;
    public event EventHandler SampleFogColorCompleteHandler;
    public event EventHandler UpdateCameraFeedHandler;

    [SerializeField]
    private bool _isSunDirection;

    private DateTime _currentTime;
    private string _currentLatitude;
    private string _currentLongitude;
    private Quaternion _currentHeadingDiff; // based on camera rotation
    private Quaternion _currentSunDirection;
    private Texture2D _currentCameraFeed;

    [SerializeField]
    private Color _currentFogColor;

    private bool _isSampleFogColor = false;
    private bool _isSampleFogColorStarted = false;

    public void ToggleSunDirection()
    {
        ToggleSunDirectionHandler?.Invoke();
    }

    internal void UpdateSunDirection()
    {
        UpdateSunDirectionHandler?.Invoke();
    }

    internal void SampleFogColorComplete()
    {
        SampleFogColorCompleteHandler?.Invoke();
    }

    internal void UpdateCameraFeed()
    {
        UpdateCameraFeedHandler?.Invoke();
    }

    public bool IsSunDirection { get => _isSunDirection; set => _isSunDirection = value; }
    public DateTime CurrentTime { get => _currentTime; set => _currentTime = value; }
    public string CurrentLatitude { get => _currentLatitude; set => _currentLatitude = value; }
    public string CurrentLongitude { get => _currentLongitude; set => _currentLongitude = value; }
    public Quaternion CurrentHeadingDiff { get => _currentHeadingDiff; set => _currentHeadingDiff = value; }
    public Quaternion CurrentSunDirection { get => _currentSunDirection; set => _currentSunDirection = value; }
    public Texture2D CurrentCameraFeed { get => _currentCameraFeed; set => _currentCameraFeed = value; }
    public Color CurrentFogColor { get => _currentFogColor; set => _currentFogColor = value; }
    public bool IsSampleFogColor { get => _isSampleFogColor; set => _isSampleFogColor = value; }
    public bool IsSampleFogColorStarted { get => _isSampleFogColorStarted; set => _isSampleFogColorStarted = value; }
}
