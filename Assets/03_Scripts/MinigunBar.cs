using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigunBar : Rewind
{
    [SerializeField] Image _minigunBar;

    [Tooltip("Cuanto aumenta la barra cuando golpea a un enemigo")]
    public float enemyAttackPoints=1f;

    [Tooltip("Cuanto aumenta la barra cuando golpea a una caja o rompe algo")]
    public float attackThingsPoints=0.5f;

    //[Tooltip("Cuanto aumenta la barra cuando mata a un enemigo")]
    //public float killsPoints=0.3f;

    [Tooltip("Cuanto dura el modo con la minigun")]
    public float maxBarTime=10f;

    private float _actualBarTime = 0f;

    public ModelMonkey modelMonkey;

    private void Start()
    {
        _minigunBar = GetComponent<Image>();
        //modelMonkey = GameManager.instance.players[0].GetComponent<ModelMonkey>();
    }

    public void PunchEnemy()
    {
        _actualBarTime += enemyAttackPoints;
    }

    public void PunchThings()
    {
        _actualBarTime += attackThingsPoints;
    }

    //public void KillEnemies()
    //{
    //    _minigunBar.fillAmount += killsPoints;
    //}

    private void Update()
    {
        if (Time.timeScale == 0) return;

        _minigunBar.fillAmount = _actualBarTime / maxBarTime;

        if (_actualBarTime>=maxBarTime)
        {
            _actualBarTime = maxBarTime;
            //modelMonkey.canActivateMinigun = true;
            Debug.Log("Estoy en modo asalto");
        }

        //if (modelMonkey.actualStateBongo == EstadoDeBongo.Minigun)
        //{
        //    _actualBarTime-= Time.deltaTime;

        //    if (_actualBarTime <= 0)
        //    {
        //        _actualBarTime = 0;
        //        //modelMonkey.canActivateMinigun = false;
        //        //modelMonkey.DesactiveMinigun();
        //        Debug.Log("ya no estoy mas en modo asalto");
        //    }
        //}
    }

    public override void Save()
    {
        _currentState.Rec(_actualBarTime);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        _actualBarTime = (float)col.parameters[0];

    }
}
