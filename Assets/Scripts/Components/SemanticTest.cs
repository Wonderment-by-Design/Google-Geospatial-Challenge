using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SemanticTest : MonoBehaviour
{
    [SerializeField]
    private ARSemanticManager _semanticManager;
    [SerializeField]
    private MeshRenderer _semanticMeshRenderer;

    private LabelColorManager _labelColorManager = new LabelColorManager();

    private Texture2D _semanticTexture;

    private void Start()
    {
        _semanticMeshRenderer.material.SetColorArray("_LabelColorArray", _labelColorManager.GetColorArray());
    }

    void Update()
    {
        if(_semanticManager.IsSemanticModeSupported(SemanticMode.Enabled) == FeatureSupported.Supported)
        {
            if (_semanticManager.TryGetSemanticTexture(ref _semanticTexture))
            {
                _semanticMeshRenderer.material.mainTexture = _semanticTexture;

                ResizeQuadToScreen(_semanticTexture.width, _semanticTexture.height);
            }
        }

    }

    void ResizeQuadToScreen(int imgWidth, int imgHeight)
    {
        //Aspect ratio of screen (height / width of Landscape image)
        float imageAspectRatio = (float)imgHeight / (float)imgWidth;
        //Convert camera's field of view to radian.
        float cameraFovRad = Camera.main.fieldOfView * Mathf.Deg2Rad;
        // Calculate the height of the Quad based on the distance from the camera to the Quad in the vertical direction.
        float quadHeightAtDistance = 2.0f * _semanticMeshRenderer.gameObject.transform.localPosition.z * Mathf.Tan(cameraFovRad / 2.0f);
        //Calculate the width of the Quad from the height of the Quad using the aspect ratio.
        float quadWidthAtDistance = quadHeightAtDistance * imageAspectRatio;
        //Apply the calculated width and height to the Quad.
        _semanticMeshRenderer.gameObject.transform.localScale = new Vector3(quadWidthAtDistance, quadHeightAtDistance, 1);
    }
}
