using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconView : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _hologram;
    [SerializeField]
    private MeshRenderer _groundLight;
    [SerializeField]
    private GameObject _indicator;

    private float _distanceThreshold = 50;
    private float _distanceRange = 50;

    private PlayerModel _playerModel;

    private Material _materialHologram;
    private Material _materialGroundLight;
    private Color _color;

    private void Awake()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        
        if (_hologram)
        {
            _materialHologram = _hologram.material;
            _color = _materialHologram.GetColor("_BaseColor");
        }

        if (_groundLight)
        {
            _materialGroundLight = _groundLight.material;
        }
    }

    private void Update()
    {
        if(_playerModel && _materialHologram)
        {
            float distance = Mathf.Abs(Vector3.Distance(transform.position, _playerModel.CurrentPosition));

            if(distance < _distanceThreshold)
            {
                _hologram.enabled = false;
                _groundLight.enabled = false;

                if(_indicator)
                    _indicator.SetActive(false);

            }
            else
            {
                _hologram.enabled = true;
                _groundLight.enabled = true;

                if (_indicator)
                {
                    _indicator.SetActive(true);
                }

                if (distance < _distanceThreshold + _distanceRange)
                {
                    _color.a = (distance - _distanceThreshold) / _distanceRange;
                    _materialHologram.SetColor("_BaseColor", _color);

                    _color.a /= 4;
                    _materialGroundLight.SetColor("_BaseColor", _color);
                }

            }
        }
    }
}
