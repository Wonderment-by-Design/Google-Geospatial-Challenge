using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class MapUIView : MonoBehaviour
{
    MapModel mapModel;

    [SerializeField]
    TextMeshProUGUI distanceText;
    [SerializeField]
    RectTransform transition;
    Image transitionImage;
    Color startColor;
    Vector2 startPos, startSize;
    private void OnEnable()
    {
        if (MapModel.Instance == null)
        {
            return;
        }

        if (mapModel == null)
        {
            mapModel = MapModel.Instance;

            mapModel.SetDistanceHandler += SetDistance;
            mapModel.SelectedHandler += Activate;
            mapModel.DeselectedHandler += Deactivate;
            transitionImage = transition.GetComponent<Image>();
            startColor = transitionImage.color;
            startPos = transition.anchoredPosition;
            startSize = transition.sizeDelta;
        }

        mapModel.MapCam.enabled = false;
        DoTransition();
    }

    void ResetValues()
    {
        transition.sizeDelta = startSize;
        transition.anchoredPosition = startPos;
        transitionImage.color = startColor;
    }

    void DoTransition()
    {
        if (transition.sizeDelta != startSize)
        {
            ResetValues();
        }

        transition.DOSizeDelta(new Vector2(1000, 1000), 0.3f);
        transition.DOAnchorPos(new Vector2(0, 421), 0.3f).OnComplete(() =>
        {
            transitionImage.DOFade(0f, 0.3f);
            mapModel.MapCam.enabled = true;
        });
    }

    void SetDistance()
    {
        distanceText.text = MapController.Instance.ConvertDistanceToString(mapModel.DistanceToTravel);
    }

    void Activate()
    {
        distanceText.gameObject.SetActive(true);
    }

    void Deactivate()
    {
        distanceText.gameObject.SetActive(false);
    }

}
