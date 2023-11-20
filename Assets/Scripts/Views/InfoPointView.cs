using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InfoPointView : MonoBehaviour
{
    [SerializeField]
    private GameObject _screen;
    [SerializeField]
    private GameObject _groundLight;

    [SerializeField]
    GameObject info;
    [SerializeField]
    GameObject i;
    [SerializeField]
    GameObject infoText;

    private PlayerModel _playerModel;
    private float _distanceThreshold = 50;

    bool found = false;

    Vector3 closedScale = new Vector3(1, 0, 1);
    bool showingInfo = false;

    public bool ShowingInfo { get => showingInfo; set => showingInfo = value; }
    public bool Found { get => found; set => found = value; }

    private void Awake()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
    }

    private void Start()
    {
        info.transform.localScale = Vector3.one;
        i.SetActive(true);
        infoText.SetActive(false);
    }

    private void Update()
    {
        if (_playerModel && _screen && _groundLight)
        {
            float distance = Mathf.Abs(Vector3.Distance(transform.position, _playerModel.CurrentPosition));

            if (distance > _distanceThreshold)
            {
                _screen.SetActive(false);
                _groundLight.SetActive(false);
            }
            else
            {
                _screen.SetActive(true);
                _groundLight.SetActive(true);
            }
        }
    }

    public void ShowInfo()
    {
        found = true;
        info.transform.DOScale(closedScale, 1).SetEase(Ease.InBack).OnComplete(() =>
        {
            i.SetActive(false);
            infoText.SetActive(true);
            info.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack);
            ShowingInfo = true;
        });     
    }

    public void StopShowingInfo()
    {
        info.transform.DOScale(closedScale, 1).SetEase(Ease.InBack).OnComplete(() =>
        {
            i.SetActive(true);
            infoText.SetActive(false);
            info.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack);
            ShowingInfo = false;
        });
    }
}
