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

    [Tooltip("Referencia de los mejores tiempos de JSON")]private TimeChronometer[] _myRefOfTheBestTimes;
    TimeChronometer _myOldRecordTime = default; //Varaible auxiliar que me va a ayudar para guardar el anterior record y posicionarlo abajo.

    private void Awake()
    {
        if(!GameManager.instance.viewBestTimesInlevel) GameManager.instance.viewBestTimesInlevel = this;

        if(_buttonBackToMenu.gameObject.activeInHierarchy) _buttonBackToMenu.gameObject.SetActive(false);

        _myRefOfTheBestTimes = GameManager.instance.currentLevel.bestTimesJSON;

        UpdateRecordsTxt();
    }

    /// <summary>
    /// Muestro las tablas de timepo en pantalla
    /// </summary>
    public void Show()
    {

        //Recorro los mejores tiempos del nivel actual
        for (int i = 0; i < _myRefOfTheBestTimes.Length; i++)
        {
            //Si el tiempo de ahora es mejor que el que está guardado, lo sobrescribo
            if(GameManager.instance.timeInLevel.timeInSeconds < _myRefOfTheBestTimes[i].timeInSeconds)
            {
                _myOldRecordTime = _myRefOfTheBestTimes[i];//Me guardo la referencia vieja

                GameManager.instance.currentLevel.SaveBestTime(i, GameManager.instance.timeInLevel);

                UpdateRecordsTime(i);

                break;
            }
        }

        _buttonBackToMenu.gameObject.SetActive(true);

        //Muestro la tabla en pantalla
        foreach (var txt in bestTimesView) txt.nameRecord.gameObject.SetActive(true);
    }

    /// <summary>
    /// Actualizar el texto de los records
    /// </summary>
    private void UpdateRecordsTxt()
    {         
        for (int i = 0; i < bestTimesView.Length; i++)
        {
            //Seteo cada texto con el nombre del json y su tiempo en formato de texto.
            bestTimesView[i].nameRecord.text = GameManager.instance.currentLevel.bestTimesJSON[i].name;
            bestTimesView[i].timeRecord.text = GameManager.instance.currentLevel.bestTimesJSON[i].timeInMinutesTxt;

            //Si esta encendido en jerarquia, lo apago
            if (bestTimesView[i].nameRecord.gameObject.activeInHierarchy) bestTimesView[i].nameRecord.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Actualizar tabla de los mejores tiempos segun su tiempo
    /// </summary>
    /// <param name="currentIndex"></param>
    private void UpdateRecordsTime(int currentIndex)
    {
        int nextIndex = currentIndex++;

        //Recorro el próximo index que me mandaron por parámetro
        for (int i = nextIndex; i < _myRefOfTheBestTimes.Length; i++)
        {
            TimeChronometer aux = default;

            //Si la referencia vieja es menor tiempo que el próximo index
            if(_myOldRecordTime.timeInSeconds < _myRefOfTheBestTimes[i].timeInSeconds)
            {
                aux = _myRefOfTheBestTimes[i];

                GameManager.instance.currentLevel.SaveBestTime(i, _myOldRecordTime); //Lo guardo en su lugar

                _myOldRecordTime = aux;
            }

        }

        //Actualizo los textos
        UpdateRecordsTxt();
    }
}
