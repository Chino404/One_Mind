using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICollectible : MonoBehaviour
{
    [SerializeField] private CharacterTarget _player;
    private string _key;

    [Space(10), SerializeField] private Color _deactiveColor;
    [SerializeField] private Color _activeColor;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();  
    }

    private void Start()
    {
        if (_player == CharacterTarget.Bongo)
        {
            _key = "BongoTrinket";
            CollectibleManager.instance.bongoUI = this;
        }
        else
        {
            _key = "FrankTrinket";
            CollectibleManager.instance.frankUI = this;
        }

        if (!CallJson.instance.refJasonSave.GetValueCollectableDict(CollectibleManager.instance.buildIndexLevel, _key)) _image.color = _deactiveColor;
        else _image.color = _activeColor;
    }

    public void CollectibleTaken()
    {
        _image.color = _activeColor;
    }
}
