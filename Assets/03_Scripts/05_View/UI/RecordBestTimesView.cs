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
    [SerializeField, Tooltip("Mi tiempo en este nivel")] private TextMeshProUGUI _myTimeInLevel;
    public ViewRecord[] bestTimesView;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI _TMPGoodLuckNextTime;

    private InputYourName _refInputName;

    private void Awake()
    {
        _refInputName = GetComponent<InputYourName>();

        if(!GameManager.instance.UIBestTimesInlevel) GameManager.instance.UIBestTimesInlevel = this;

        //Desactivo todo lo que este activado por error
        if(_buttonBackToMenu.gameObject.activeInHierarchy) _buttonBackToMenu.gameObject.SetActive(false);
        if(_myTimeInLevel.gameObject.activeInHierarchy) _myTimeInLevel.gameObject.SetActive(false);

        foreach (var txt in bestTimesView) if (txt.nameRecord.gameObject.activeInHierarchy) txt.nameRecord.gameObject.SetActive(false);

        if(_refInputName.inputField.gameObject.activeInHierarchy) _refInputName.inputField.gameObject.SetActive(false);

        UpdateRecordsTxt();
    }

    /// <summary>
    /// Muestro las tablas de timepo en pantalla
    /// </summary>
    public void Show()
    {
        //Si hayun nuevo record, llamo al método para poner mi nombre
        if (GameManager.instance.currentLevel.CheckNewTimeWithTheBestTimes(GameManager.instance.timeInLevel, false)) NewRecord();

        //Sino solo muestro un msj y la tabla de puntuación
        else
        {
            _TMPGoodLuckNextTime.gameObject.SetActive(true);
            ShowBestTimesRecords();
        }

        _myTimeInLevel.text = GameManager.instance.timeInLevel.txtTimeInMinutes;
        _myTimeInLevel.gameObject.SetActive(true);

        //Muestro la tabla en pantalla
        foreach (var txt in bestTimesView) txt.nameRecord.gameObject.SetActive(true);
   
    }

    public void NewRecord() => _refInputName.inputField.gameObject.SetActive(true);


    public void ShowBestTimesRecords()
    {
        if(_refInputName.inputField.gameObject.activeInHierarchy)_refInputName.inputField.gameObject.SetActive(false);

        GameManager.instance.currentLevel.CheckNewTimeWithTheBestTimes(GameManager.instance.timeInLevel);

        if(GameManager.instance.timeInLevel.TimeInSeconds == GameManager.instance.currentLevel.bestTimesJSON[0].TimeInSeconds) GameManager.instance.currentLevel.isLevelCompleteWithChronometerJSON = true;

        _buttonBackToMenu.gameObject.SetActive(true);

        UpdateRecordsTxt();
    }

    /// <summary>
    /// Actualizar el texto de los records
    /// </summary>
    private void UpdateRecordsTxt()
    {         
        //Actualizo el texto de los records
        for (int i = 0; i < bestTimesView.Length; i++)
        {
            //Seteo cada texto con el nombre del json y su tiempo en formato de texto.
            bestTimesView[i].nameRecord.text = GameManager.instance.currentLevel.bestTimesJSON[i].name;
            bestTimesView[i].timeRecord.text = GameManager.instance.currentLevel.bestTimesJSON[i].txtTimeInMinutes;
        }

    }
}
