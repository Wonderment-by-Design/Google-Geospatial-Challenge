using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectableView : MonoBehaviour
{
    [SerializeField]
    private GameObject _go;
    [SerializeField]
    private GameObject _forcefield;

    private PlayerModel _playerModel;
    private float _distanceThreshold = 50;

    private bool canBeCollected = true;

    private Material _forceFieldMaterial;

    private void Awake()
    {
        _playerModel = FindObjectOfType<PlayerModel>();

        if (_forcefield)
            _forceFieldMaterial = _forcefield.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (_playerModel && _go)
        {
            float distance = Mathf.Abs(Vector3.Distance(transform.position, _playerModel.CurrentPosition));

            if (distance > _distanceThreshold)
            {
                _go.SetActive(false);
            }
            else
            {
                _go.SetActive(true);

                Color forceFieldColor = _forceFieldMaterial.GetColor("_BaseColor");
                forceFieldColor.a = 1 - (distance / _distanceThreshold);
                _forceFieldMaterial.SetColor("_BaseColor", forceFieldColor);
            }
        }
    }

    public void Collect()
    {
        Debug.Log("collected me");
        canBeCollected = false;

        transform.DORotate(new Vector3(0, 500, 0), 0.85f,RotateMode.FastBeyond360).SetEase(Ease.InBack);
        transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBack).OnComplete(() => { gameObject.SetActive(false); });
    }

    public bool CanBeCollected { get => canBeCollected; set => canBeCollected = value; }
}
