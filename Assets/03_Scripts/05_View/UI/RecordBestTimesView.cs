using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[Serializable]
public struct ViewRecord
{
    public TextMeshProUGUI nameRecord;
    public TextMeshProUGUI timeRecord;
}

public class RecordBestTimesView : MonoBehaviour
{
    [SerializeField]private Button _buttonBackToMenu;
    public ViewRecord[] bestTimesView;

    private void Awake()
    {
        if(!GameManager.instance.viewBestTimesInlevel) GameManager.instance.viewBestTimesInlevel = this;

        if(_buttonBackToMenu.gameObject.activeInHierarchy) _buttonBackToMenu.gameObject.SetActive(false);


        UpdateRecords();
    }

    /// <summary>
    /// Muestro las tablas de timepo en pantalla
    /// </summary>
    public void Show()
    {
        var bestTimesRecords = GameManager.instance.currentLevel.bestTimes;

        //Recorro los mejores tiempos del nivel actual
        for (int i = 0; i < bestTimesRecords.Length; i++)
        {
            TimeChronometer aux = default; //Varaible auxiliar que ,e va a ayudar para guardar el anterior record y posicionarlo abajo.


            //Si el tiempo de ahora es mejor que el que está guardado, lo sobrescribo
            if(GameManager.instance.timeInLevel.time < bestTimesRecords[i].time)
            {
                GameManager.instance.currentLevel.SaveBestTime(i, GameManager.instance.timeInLevel);

                UpdateRecords();

                break;
            }
        }

        _buttonBackToMenu.gameObject.SetActive(true);

        //Muestro la tabla en pantalla
        foreach (var txt in bestTimesView) txt.nameRecord.gameObject.SetActive(true);
    }

    /// <summary>
    /// Actualizar laos records
    /// </summary>
    private void UpdateRecords()
    {
        for (int i = 0; i < bestTimesView.Length; i++)
        {
            //Seteo cada texto con el nombre del json y su tiempo en formato de texto.
            bestTimesView[i].nameRecord.text = GameManager.instance.currentLevel.bestTimes[i].name;
            bestTimesView[i].timeRecord.text = GameManager.instance.currentLevel.bestTimes[i].timeInMinutesTxt;

            //Si esta encendido en jerarquia, lo apago
            if (bestTimesView[i].nameRecord.gameObject.activeInHierarchy) bestTimesView[i].nameRecord.gameObject.SetActive(false);
        }
    }
}
