using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ObstacleView : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    //[SerializeField]
    //private GameObject _top;
    [SerializeField]
    private GameObject _forcefield;
    [SerializeField]
    private GameObject _groundLight;

    private RobotModel _robotModel;
    private PlayerModel _playerModel;
    private ObstacleModel _obstacleModel;
    private ObstacleController _obstacleController;

    private ObstacleVO _VO = new ObstacleVO();
    private Material _forceFieldMaterial;
    private Material _groundLightMaterial;

    //private SplineContainer _splineContainer;

    private void Awake()
    {
        _obstacleModel = FindObjectOfType<ObstacleModel>();
        //_obstacleModel.TriggerSpawnHandler += TriggerSpawnHandler;
        _obstacleController = FindObjectOfType<ObstacleController>();

        _playerModel = FindObjectOfType<PlayerModel>();
        if(_playerModel)
            _playerModel.FoundObstacleHandler += FoundObstacleHandler;

        _robotModel = FindObjectOfType<RobotModel>();
        //_robotModel.TriggerSpawnPickupCompleteHandler += TriggerSpawnPickupCompleteHandler;
        if (_robotModel)
        {
            _robotModel.UpdateAnimationStateHandler += UpdateAnimationStateHandler;
            _robotModel.UpdateHandTransformHandler += UpdateHandTransformHandler;
            _robotModel.SpawnThrowCompleteHandler += SpawnThrowCompleteHandler;
        }

        if(_forcefield)
            _forceFieldMaterial = _forcefield.GetComponent<MeshRenderer>().material;

        if( _groundLight)
            _groundLightMaterial = _groundLight.GetComponent<MeshRenderer>().material;

        //_splineContainer = GetComponentInChildren<SplineContainer>();
    }

    private void Start()
    {
        gameObject.SetActive(false);

        ForceFieldHide();
    }

    private void FoundObstacleHandler(ObstacleView obstacleView)
    {
        if (obstacleView == this)
        {
            _VO.CurrentState = ObstacleVO.State.Found;
            _VO.TargetPosition = transform.position;
            _VO.TargetRotation = transform.rotation;
            //Logger.Add("ObstacleView: FoundObstacleHandler: " + _VO.CurrentState);
        }
    }

    //private void TriggerSpawnHandler(ObstacleView obstacleView)
    //{
    //    if (_obstacleModel.CurrentObstacleView == this)
    //    {
    //        _VO.CurrentState = ObstacleVO.State.AnimationPickUp;
    //        Logger.Add("ObstacleView: TriggerSpawnHandler: " + _VO.CurrentState);
    //    }
    //}

    // During pickup the current obstacle is moved to the robot hand position + rotation
    private void UpdateHandTransformHandler()
    {
        if (_obstacleModel.CurrentObstacleView == this)
        {
            transform.position = _robotModel.CurrentHandPosition;
            transform.rotation = _robotModel.CurrentHandRotation;
        }
    }

    private void UpdateAnimationStateHandler()
    {
        if (_obstacleModel.CurrentObstacleView == this)
        {
            if(_robotModel.CurrentAnimationState == RobotModel.AnimationState.PickUp)
            {
                gameObject.SetActive(true);
                _VO.CurrentState = ObstacleVO.State.AnimationPickUp;
            }
            else if(_robotModel.CurrentAnimationState == RobotModel.AnimationState.Throw)
            {
                _VO.CurrentState = ObstacleVO.State.AnimationThrow;

                //_obstacleController.InitObstacleSpline();
            }
        }
    }

    private void Update()
    {
        if(_obstacleModel)
        {
            if (_obstacleModel.CurrentObstacleView == this && _robotModel.CurrentAnimationState == RobotModel.AnimationState.Throw)
            {
                UpdateSpawnAnimation();
            }

            UpdateForceField();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ForceFieldOpen();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ForceFieldClose();
        }
    }

    private void UpdateSpawnAnimation()
    {
        _VO.SplineRatio += Time.deltaTime / _robotModel.SpawnThrowAnimationTime;

        transform.position = _obstacleModel.CurrentSpline.EvaluatePosition(_obstacleModel.SplineAnimationCurve.Evaluate(_VO.SplineRatio));
        transform.rotation = Quaternion.Lerp(_robotModel.CurrentHandRotation, _VO.TargetRotation, _VO.SplineRatio);

        if (_VO.SplineRatio > 1.0f)
        {
            SpawnComplete();
        }
    }

    private void SpawnThrowCompleteHandler()
    {
        if (_obstacleModel.CurrentObstacleView == this)
        {
            SpawnComplete();
        }
    }

    private void SpawnComplete()
    {
        _VO.SplineRatio = 1.0f;

        transform.position = _VO.TargetPosition;
        transform.rotation = _VO.TargetRotation;

        _VO.CurrentState = ObstacleVO.State.Spawned;
    }

    private void ForceFieldOpen()
    {
        Invoke("ForceFieldShow", 1);

        _animator.SetBool("IsOpen", true);
        _VO.CurrentState = ObstacleVO.State.ForceFieldOpen;
    }

    private void ForceFieldClose()
    {
        ForceFieldHide();

        _animator.SetBool("IsOpen", false);
        _VO.CurrentState = ObstacleVO.State.ForceFieldClose;
    }

    private void ForceFieldShow()
    {
        _forcefield.SetActive(true);
        _groundLight.SetActive(true);
    }

    private void ForceFieldHide()
    {
        _forcefield.SetActive(false);
        _groundLight.SetActive(false);
    }

    private void UpdateForceField()
    {
        if(VO.Distance < _obstacleModel.ForceFieldThresholdRange)
        {
            if(_VO.CurrentState == ObstacleVO.State.Spawned || _VO.CurrentState == ObstacleVO.State.ForceFieldClose)
                ForceFieldOpen();

            Color forceFieldColor = _forceFieldMaterial.GetColor("_BaseColor");
            forceFieldColor.a = 1 - (VO.Distance / _obstacleModel.ForceFieldThresholdRange);
            _forceFieldMaterial.SetColor("_BaseColor", forceFieldColor);

            Color groundLightColor = _groundLightMaterial.GetColor("_BaseColor");
            groundLightColor.a = 1 - (VO.Distance / _obstacleModel.ForceFieldThresholdRange / 2);
            groundLightColor.a /= 4;
            _groundLightMaterial.SetColor("_BaseColor", groundLightColor);
        }
        else
        {
            if (_VO.CurrentState == ObstacleVO.State.Spawned || _VO.CurrentState == ObstacleVO.State.ForceFieldOpen)
                ForceFieldClose();
        }
    }

    public ObstacleVO VO { get => _VO; set => _VO = value; }
}
