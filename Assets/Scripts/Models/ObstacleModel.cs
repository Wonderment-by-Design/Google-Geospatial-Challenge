using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ObstacleModel : MonoBehaviour
{
    public delegate void EventObstacleHandler(ObstacleView obstacleView);
    public event EventObstacleHandler TriggerSpawnHandler;

    [SerializeField]
    private SplineContainer _splineContainer;
    [SerializeField]
    private Vector3 _splineBezierOffset = Vector3.zero; // controls bezier curve offset
    [SerializeField]
    private AnimationCurve _splineAnimationCurve;
    [SerializeField]
    private float _forceFieldThresholdRange; // controls the additive blend of the forcefield based on distance from player

    private ObstacleView[] _obstacleViews;
    private List<ObstacleView> _queu = new List<ObstacleView>();
    //private List<SplineContainer> _splines = new List<SplineContainer>();
    //private List<ObstacleView> _spawned = new List<ObstacleView>();

    private ObstacleView _currentObstacleView;
    private Spline _currentSpline;

    //internal void AddToSpawnQueu(ObstacleView obstacleView)
    //{
    //    _queu.Add(obstacleView);
    //}

    internal void TriggerNextSpawn()
    {
        TriggerSpawnHandler?.Invoke(_currentObstacleView);

        Logger.Add("Trigger next spawn");
        Logger.Add("Current count: " + Queu.Count);
    }

    public SplineContainer SplineContainer { get => _splineContainer; set => _splineContainer = value; }
    public ObstacleView[] ObstacleViews { get => _obstacleViews; set => _obstacleViews = value; }
    public ObstacleView CurrentObstacleView { get => _currentObstacleView; set => _currentObstacleView = value; }
    public List<ObstacleView> Queu { get => _queu; set => _queu = value; }
    public Vector3 SplineBezierOffset { get => _splineBezierOffset; set => _splineBezierOffset = value; }
    public AnimationCurve SplineAnimationCurve { get => _splineAnimationCurve; set => _splineAnimationCurve = value; }
    //public List<SplineContainer> Splines { get => _splines; set => _splines = value; }
    public Spline CurrentSpline { get => _currentSpline; set => _currentSpline = value; }
    public float ForceFieldThresholdRange { get => _forceFieldThresholdRange; set => _forceFieldThresholdRange = value; }

    //public List<ObstacleView> Queued { get => _queued; set => _queued = value; }
    //public List<ObstacleView> Spawned { get => _spawned; set => _spawned = value; }
    //public float ThresholdRange { get => _thresholdRange; set => _thresholdRange = value; }
}
