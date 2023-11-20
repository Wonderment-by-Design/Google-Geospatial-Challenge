using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ARMapUIView : MonoBehaviour
{
    MapModel mapModel;
    LabyrinthModel labyrinthModel;

    [SerializeField]
    TextMeshProUGUI distanceText;
    [SerializeField]
    RectTransform movingObjects;
    [SerializeField]
    RectTransform transition;
    [SerializeField]
    GameObject startPanel;
    [SerializeField]
    TextMeshProUGUI countDownText;
    bool showingMenu = false;
    bool countDownStarted = false;
    Image transitionImage;
    Color startColor;
    Vector2 startPos, startSize;
    WaitForSeconds oneSecond;

    private void Start()
    {
        oneSecond = new WaitForSeconds(1);
    }

    private void OnEnable()
    {
        if (MapModel.Instance == null)
        {
            return;
        }

        if (mapModel == null)
        {
            mapModel = MapModel.Instance;
            labyrinthModel = LabyrinthModel.Instance;

            mapModel.SetDistanceHandler += SetDistance;
            mapModel.SelectedHandler += Activate;
            mapModel.DeselectedHandler += Deactivate;
            labyrinthModel.ToggleStartHandler += ToggleStart;
            transitionImage = transition.GetComponent<Image>();
            startColor = transitionImage.color;
            startPos = transition.anchoredPosition;
            startSize = transition.sizeDelta;
        }

        SetDistance();
        DoTransition();
    }

    void ToggleStart()
    {
        if (countDownStarted)
        {
            startPanel.SetActive(false);
            return;
        }
        startPanel.SetActive(labyrinthModel.AllowedToStart);
    }

    public void StartCountDown()
    {
        countDownStarted = true;
        ToggleStart();

        countDownText.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countDownText.text = "3";
        yield return oneSecond;
        countDownText.text = "2";
        yield return oneSecond;
        countDownText.text = "1";
        yield return oneSecond;
        countDownText.text = "GO!";
        yield return oneSecond;
        countDownText.gameObject.SetActive(false);
        LabyrinthController.Instance.StartLabyrinth();
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

        transition.DOSizeDelta(new Vector2(636, 374), 0.3f);
        transition.DOAnchorPos(new Vector2(0, 0), 0.3f).OnComplete(() => { transitionImage.DOFade(0f, 0.3f); });
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

    public void ToggleMenu()
    {
        if (showingMenu)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    void ShowMenu()
    {
        movingObjects.DOAnchorPosY(86f, 0.6f).SetEase(Ease.InCubic);
        showingMenu = true;
    }

    void HideMenu()
    {
        movingObjects.DOAnchorPosY(0, 0.6f).SetEase(Ease.InCubic);
        showingMenu = false;
    }
}
