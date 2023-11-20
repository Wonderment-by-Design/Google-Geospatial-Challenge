using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

// trigger animation
// timeout throw
// right hand position
// target position
// throw speed

public class ThrowComponent : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SplineContainer splineContainer;
    [SerializeField]
    private float splineRatio;

    [SerializeField]
    private Vector3 bezierOffset;

    [SerializeField]
    private GameObject rightHand;
    [SerializeField]
    private GameObject targetWall;

    [SerializeField]
    private float timeoutThrow = 1.0f;

    [SerializeField]
    private float throwDuration = 1.0f;

    private Vector3 _sourcePosition;
    private Quaternion _sourceRotation;

    private Vector3 _targetPosition;
    private Quaternion _targetRotation;

    private bool _isPickingUp = false;
    private bool _isThrowing = false;

    private void Start()
    {
        targetWall.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Throw();
        }

        if(splineContainer.Splines.Count > 0 && splineContainer.Splines[0].Knots.Count<BezierKnot>() > 0)
        {
            targetWall.transform.position = splineContainer.Splines[0].EvaluatePosition(splineRatio);
        }

        if (_isPickingUp)
        {
            targetWall.SetActive(true);
            targetWall.transform.position = rightHand.transform.position;
        }

        if (_isThrowing)
        {
            splineRatio += Time.deltaTime / throwDuration;

            targetWall.transform.rotation = Quaternion.Lerp(_sourceRotation, _targetRotation, splineRatio);

            if (splineRatio > 1.0f)
            {
                _isThrowing = false;
                splineRatio = 1.0f;

                targetWall.transform.rotation = _targetRotation;
            }
        }

    }

    public void Throw()
    {
        _isPickingUp = true;

        InitThrow();

        Invoke("TriggerThrow", timeoutThrow);
    }

    private void InitThrow()
    {
        _targetPosition = targetWall.transform.position;
        _targetRotation = targetWall.transform.rotation;

        animator.SetTrigger("Throw");
    }

    private void TriggerThrow()
    {
        _isPickingUp = false;
        _isThrowing = true;

        _sourcePosition = rightHand.transform.position;
        _sourceRotation = rightHand.transform.rotation;

        InitSpline();
    }

    private void InitSpline()
    {
        splineRatio = 0.0f;

        splineContainer.Splines[0].Clear();

        BezierKnot knot;
        knot = new BezierKnot();
        knot.Position = _sourcePosition;
        splineContainer.Splines[0].Add(knot);

        knot = new BezierKnot();
        knot.Position = _targetPosition + bezierOffset;
        splineContainer.Splines[0].Add(knot);

        knot = new BezierKnot();
        knot.Position = _targetPosition;
        splineContainer.Splines[0].Add(knot);

        splineContainer.Splines[0].SetTangentMode(TangentMode.AutoSmooth);
    }
}
