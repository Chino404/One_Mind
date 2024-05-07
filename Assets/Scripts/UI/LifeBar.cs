using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Image _lifeBar;

    private void Awake()
    {
        _lifeBar = GetComponent<Image>();
    }

    void Start()
    {
        EventManager.Subscribe("ProjectLifeBar", ProjectLifeBar);
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
}
