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

    [SerializeField] private TextMeshProUGUI _txt;
    private int _totalAmountCollectible = 1;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _txt = GetComponentInChildren<TextMeshProUGUI>();

        //Si no hay ninguna refrencia de la UI en el GameManager, me agrego yo
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

        //Si ya hay, me destruyo
        else Destroy(gameObject);
    }

    /// <summary>
    /// Seteo el coleccionable respecto al nivel que pertenezca
    /// </summary>
    /// <param name="buildIndexLevel"></param>
    public void SetUIToLevel(int buildIndexLevel)
    {
        //Si no se agarro el coleccionable en este nivel
        if (!CallJson.instance.refJasonSave.GetValueCollectableDict(buildIndexLevel, _key))
        {
            //Desactivo el color en la UI
            _image.color = _deactiveColor;

            //Actualizo el texto a 0
            UpdateCollectibleTxt(0);
        }

        //Caso contrario
        else
        {
            _image.color = _activeColor;
            UpdateCollectibleTxt(1);
        }
    }

    /// <summary>
    /// Avisa a la UI que tome el coleccionable y lo seteo
    /// </summary>
    public void UICollectibleTaken()
    {
        _image.color = _activeColor;

        UpdateCollectibleTxt(1);
    }

    /// <summary>
    /// Actualizo el texto del coleccionable
    /// </summary>
    /// <param name="current"></param>
    private void UpdateCollectibleTxt(int current)
    {
        _txt.text = $"{current} / {_totalAmountCollectible}";
    }

}
