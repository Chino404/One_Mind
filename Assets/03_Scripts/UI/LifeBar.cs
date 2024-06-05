using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : Rewind
{
    public static LifeBar instance;
    [SerializeField] private Image _lifeBar;
    [SerializeField] Color _maxLifeColor, _mediumLifeColor, _minLifeColor;
    [SerializeField] GameObject _skullMaxLife, _skullMediumLife, _skullMinLife;

    private void Awake()
    {
        _lifeBar = GetComponent<Image>();
    }

    void Start()
    {
        EventManager.Subscribe("ProjectLifeBar", ProjectLifeBar);

    }

    private void Update()
    {
        if (_lifeBar.fillAmount == 1)
        {
            _skullMaxLife.SetActive(true);
            _lifeBar.color = _maxLifeColor;
        }

        else if(_lifeBar.fillAmount<1&&_lifeBar.fillAmount>=0.5f)
        {
            _skullMaxLife.SetActive(false);
            _skullMediumLife.SetActive(true);
            _lifeBar.color = _mediumLifeColor;
        }

        else
        {
            _skullMediumLife.SetActive(false);
            _skullMinLife.SetActive(true);
            _lifeBar.color = _minLifeColor;
        }
    }

    public void ProjectLifeBar(params object[] parameters) //El "params" antes del array me permite pasarle cosas sueltas y el me arma el array
    {
        var maxLife = (float)parameters[0];
        var actualLife = (float)parameters[1];

        _lifeBar.fillAmount = actualLife / maxLife;

        

        if (_lifeBar == null)
            _lifeBar.fillAmount = maxLife;
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("ProjectLifeBar", ProjectLifeBar);
    }

    public override void Save()
    {
        Debug.Log("guarde barra de vida");
        //EventManager.Subscribe("ProjectLifeBar", ProjectLifeBar);

        _currentState.Rec(_lifeBar.fillAmount);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        Debug.Log("cargue barra de vida");
        var col = _currentState.Remember();
        _lifeBar.fillAmount = (float)col.parameters[0];
    }
}
