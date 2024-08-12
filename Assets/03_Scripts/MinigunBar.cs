using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigunBar : MonoBehaviour
{
    [SerializeField] Image _minigunBar;

    [Tooltip("Cuanto aumenta la barra cuando golpea a un enemigo")]
    public float enemyAttackPoints=0.1f;
    [Tooltip("Cuanto aumenta la barra cuando golpea a una caja o rompe algo")]
    public float attackThingsPoints=0.05f;
    [Tooltip("Cuanto aumenta la barra cuando mata a un enemigo")]
    public float killsPoints=0.3f;
    [Tooltip("Cuanto dura el modo con la minigun")]
    public float timeInAssaultMode=0.001f;

    bool assaultMode;

    private void Start()
    {
        _minigunBar = GetComponent<Image>();
    }

    public void PunchEnemy()
    {
        _minigunBar.fillAmount += enemyAttackPoints;
    }

    public void PunchThings()
    {
        _minigunBar.fillAmount += attackThingsPoints;
    }

    public void KillEnemies()
    {
        _minigunBar.fillAmount += killsPoints;
    }

    private void Update()
    {
        if (_minigunBar.fillAmount == 1)
        {
            assaultMode = true;
            Debug.Log("Estoy en modo asalto");
        }

        if (assaultMode)
        {
            _minigunBar.fillAmount -= timeInAssaultMode;

            if (_minigunBar.fillAmount == 0)
            {
                assaultMode = false;
                Debug.Log("ya no estoy mas en modo asalto");
            }
        }
    }

}
