using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LabyrinthUIView : MonoBehaviour
{
    LabyrinthModel labyrinthModel;
    InteractiveModel interactiveModel;

    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    TextMeshProUGUI collectionText;
    [SerializeField]
    Image objectsCloseHint;
    [SerializeField]
    GameObject crosshair, textHint;
    bool hintActive = false;

    Color transparent = new Color(1, 1, 1, 0);
    Color whiteDimmed = new Color(1, 1, 1, 0.4f);

    void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
        interactiveModel = InteractiveModel.Instance;

        labyrinthModel.UpdateTimerHandler += UpdateTimer;
        interactiveModel.UpdateCollectionHandler += UpdateCollection;
        interactiveModel.ShowObjectsCloseHandler += ShowObjectsCloseHint;
        interactiveModel.HideObjectsCloseHandler += HideObjectsCloseHint;
        objectsCloseHint.color = transparent;
    }

    void ShowObjectsCloseHint()
    {
        if (hintActive)
        {
            return;
        }

        objectsCloseHint.DOColor(Color.white, 1f).OnComplete(() =>
        {
            objectsCloseHint.DOColor(whiteDimmed, 1f).SetLoops(-1, LoopType.Yoyo);
        });
        crosshair.SetActive(true);
        textHint.SetActive(true);
        hintActive = true;
    }

    void HideObjectsCloseHint()
    {
        if (!hintActive)
        {
            return;
        }

        DOTween.Kill(objectsCloseHint);
        objectsCloseHint.DOColor(transparent, 1f);
        crosshair.SetActive(false);
        textHint.SetActive(false);
        hintActive = false;
    }

    void UpdateTimer()
    {
        timerText.text = labyrinthModel.TimeText;
    }

    void UpdateCollection()
    {
        collectionText.text = "Collected:" + interactiveModel.AmountCollected.ToString();
    }
}
