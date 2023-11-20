using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotModel : MonoBehaviour
{
    public enum AnimationState { Idle, PickUp, Throw};

    public delegate void EventHandler ();
    public event EventHandler UpdateHandTransformHandler;
    public event EventHandler UpdateAnimationStateHandler;
    public event EventHandler SpawnThrowCompleteHandler;
    public event EventHandler UpdateScaleHandler;

    //public event EventHandler TriggerSpawnPickupCompleteHandler;

    [SerializeField]
    private float _spawnPickupAnimationTime = 4.5f;
    [SerializeField]
    private float _spawnThrowAnimationTime = 4.5f;

    //private List<ObstacleView> _animationQueu = new List<ObstacleView>();

    //private ObstacleView _currentObstacle;
    private AnimationState _currentAnimationState = AnimationState.Idle;
    private Vector3 _currentHandPosition;
    private Quaternion _currentHandRotation;
    private float _currentScale;

    //internal void AddToAnimationQueu(ObstacleView obstacleView)
    //{
    //    _animationQueu.Add(obstacleView);
    //}

    //internal void TriggerSpawnAnimation()
    //{

    //}

    //internal void TriggerSpawnPickupComplete()
    //{
    //    TriggerSpawnPickupCompleteHandler?.Invoke();
    //}

    internal void UpdateTransformHand()
    {
        UpdateHandTransformHandler?.Invoke();
    }

    internal void UpdateAnimationState()
    {
        UpdateAnimationStateHandler?.Invoke();
        //Logger.Add("Update animation state: " + _currentAnimationState);
    }

    internal void SpawnThrowComplete()
    {
        SpawnThrowCompleteHandler?.Invoke();
    }

    internal void UpdateScale()
    {
        UpdateScaleHandler?.Invoke();
        //Logger.Add("Update scale: " + CurrentScale);
    }

    //public List<ObstacleView> AnimationQueu { get => _animationQueu; set => _animationQueu = value; }
    //public ObstacleView CurrentObstacle { get => _currentObstacle; set => _currentObstacle = value; }
    public float SpawnPickupAnimationTime { get => _spawnPickupAnimationTime; set => _spawnPickupAnimationTime = value; }
    public AnimationState CurrentAnimationState { get => _currentAnimationState; set => _currentAnimationState = value; }
    public Vector3 CurrentHandPosition { get => _currentHandPosition; set => _currentHandPosition = value; }
    public Quaternion CurrentHandRotation { get => _currentHandRotation; set => _currentHandRotation = value; }
    public float SpawnThrowAnimationTime { get => _spawnThrowAnimationTime; set => _spawnThrowAnimationTime = value; }
    public float CurrentScale { get => _currentScale; set => _currentScale = value; }
}
