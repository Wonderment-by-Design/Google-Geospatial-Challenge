using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.Rendering.DebugUI.Table;

public class ObstacleController : MonoBehaviour
{
    //[SerializeField]
    //private GameObject _check;
            
    private ObstacleModel _obstacleModel;
    private RobotModel _robotModel;
    private PlayerModel _playerModel;

    private void Awake()
    {
        _obstacleModel = FindObjectOfType<ObstacleModel>();

        _robotModel = FindObjectOfType<RobotModel>();
        _robotModel.UpdateAnimationStateHandler += UpdateAnimationStateHandler;
        _robotModel.SpawnThrowCompleteHandler += SpawnThrowCompleteHandler;

        _playerModel = FindObjectOfType<PlayerModel>();
        _playerModel.FoundObstacleHandler += FoundObstacleHandler;

        // Find all obstacle views
        _obstacleModel.ObstacleViews = FindObjectsOfType<ObstacleView>();
        Logger.Add("Obstacles found: " + _obstacleModel.ObstacleViews.Count());
    }

    private void FoundObstacleHandler(ObstacleView obstacleView)
    {
        // Add obstacle to animation queu
        _obstacleModel.Queu.Add(obstacleView);
        //// Create spline for throw animation - Could be done via object pool?
        _obstacleModel.SplineContainer.AddSpline();

        //_obstacleModel.AddToSpawnQueu(obstacleView);

        // check if is animating. If not, start throw animation
        if (_robotModel.CurrentAnimationState == RobotModel.AnimationState.Idle)
        {
            TriggerNextSpawn();
        }
    }

    // we want to create a spline path for current obstacle when robot animation updates to throw state
    private void UpdateAnimationStateHandler()
    {
        if(_robotModel.CurrentAnimationState == RobotModel.AnimationState.Throw)
        {
            InitCurrentObstacleSplineContainer();
        }
    }

    private void InitCurrentObstacleSplineContainer()
    {
        BezierKnot knot;
        knot = new BezierKnot();
        knot.Position = _robotModel.CurrentHandPosition;
        _obstacleModel.CurrentSpline.Add(knot);

        // calculate x and z offset for middle know / bezier curve
        float dx = (_robotModel.CurrentHandPosition.x - _obstacleModel.CurrentObstacleView.VO.TargetPosition.x);
        float dz = (_robotModel.CurrentHandPosition.z - _obstacleModel.CurrentObstacleView.VO.TargetPosition.z);
        float distance = Mathf.Sqrt(dx * dx + dz * dz);
        Vector3 offset = Vector3.zero;
        offset.x = (_obstacleModel.SplineBezierOffset.x / distance) * dx;
        offset.y = _obstacleModel.SplineBezierOffset.y;
        offset.z = (_obstacleModel.SplineBezierOffset.z / distance) * dz;

        // debug check
        //_check.transform.position = _obstacleModel.CurrentObstacleView.VO.TargetPosition + offset;

        knot = new BezierKnot();
        knot.Position = _obstacleModel.CurrentObstacleView.VO.TargetPosition + offset;
        _obstacleModel.CurrentSpline.Add(knot);

        knot = new BezierKnot();
        knot.Position = _obstacleModel.CurrentObstacleView.VO.TargetPosition;
        _obstacleModel.CurrentSpline.Add(knot);

        _obstacleModel.CurrentObstacleView.VO.SplineRatio = 0.0f;

        _obstacleModel.CurrentSpline.SetTangentMode(TangentMode.AutoSmooth);
    }

    private void SpawnThrowCompleteHandler()
    {
        // remove current / first obstacle from queu
        _obstacleModel.Queu.RemoveAt(0);
        //// remove current spline from container
        _obstacleModel.SplineContainer.RemoveSplineAt(0);

        if(_obstacleModel.Queu.Count > 0)
        {
            TriggerNextSpawn();
        }
    }

    private void TriggerNextSpawn()
    {
        _obstacleModel.CurrentObstacleView = _obstacleModel.Queu[0];
        _obstacleModel.CurrentSpline = _obstacleModel.SplineContainer.Splines[0];

        _obstacleModel.TriggerNextSpawn();
    }

}
