using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICollectible : MonoBehaviour
{
    [SerializeField] private CharacterTarget _player;
    private string _key;

    [Space(10), SerializeField] private Color _deactiveColor;
    [SerializeField] private Color _activeColor;

    private Image _image;

    [SerializeField]private TextMeshProUGUI _txt;
    private int _totalAmountCollectible = 1;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _txt = GetComponentInChildren<TextMeshProUGUI>();

        if (_player == CharacterTarget.Bongo && !GameManager.instance.UIBongoTrincket)
        {
            GameManager.instance.UIBongoTrincket = this;
            _key = "BongoTrinket";
        }

        else if (_player == CharacterTarget.Frank && !GameManager.instance.UIFrankTrincket)
        {
            GameManager.instance.UIFrankTrincket = this;
            _key = "FrankTrinket";
        }

        else Destroy(gameObject);
    }

    public void SetUIToLevel(int buildIndexLevel)
    {
        if (!CallJson.instance.refJasonSave.GetValueCollectableDict(buildIndexLevel, _key))
        {
            _image.color = _deactiveColor;
            UpdateCollectibleTxt(0);
        }

        else
        {
            _image.color = _activeColor;
            UpdateCollectibleTxt(1);
        }
    }

    public void CollectibleTaken()
    {
        _image.color = _activeColor;
        UpdateCollectibleTxt(1);
    }

    private void UpdateCollectibleTxt(int current)
    {
        _txt.text = $"{current} / {_totalAmountCollectible}";
    }

}
