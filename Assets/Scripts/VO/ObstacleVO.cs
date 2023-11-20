using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleVO
{
    public enum State { Hidden, Found, AnimationPickUp, AnimationThrow, Spawned, ForceFieldOpen, ForceFieldClose};

    public State CurrentState = State.Hidden;

    public Vector3 TargetPosition;
    public Quaternion TargetRotation;

    public float SplineRatio = 0; // controls the position on the spline, value 0 - 1
    public float Distance; // distance from player
}
