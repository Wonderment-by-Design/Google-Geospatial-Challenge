using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionVisualView : MonoBehaviour
{
    [SerializeField]
    Image loadVisual;

    private void Start()
    {
        loadVisual.fillAmount = 0;
    }

    private void Update()
    {
        if(InteractiveModel.Instance != null && InteractiveController.Instance != null)
        {
            loadVisual.fillAmount = Mathf.Lerp(0, InteractiveModel.Instance.TimeToInteract, InteractiveModel.Instance.InteractionVisualValue);
        }
    }

}
