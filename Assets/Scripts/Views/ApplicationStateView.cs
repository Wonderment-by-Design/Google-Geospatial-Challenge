using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationStateView : MonoBehaviour
{
    [SerializeField]
    private ApplicationState[] _activeApplicationStates;

    private ApplicationModel _applicationModel;

    void Awake()
    {
        _applicationModel = FindObjectOfType<ApplicationModel>();
        _applicationModel.ChangeStateHandler += ChangeStateHandler;
    }

    void ChangeStateHandler()
    {
        gameObject.SetActive(_activeApplicationStates.Contains(_applicationModel.CurrentState));
    }
}
