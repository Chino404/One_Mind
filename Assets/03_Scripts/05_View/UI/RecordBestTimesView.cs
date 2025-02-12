using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[Serializable]
public struct ViewRecord
{
    public TextMeshProUGUI nameRecord;
    public TextMeshProUGUI timeRecord;
}

public class RecordBestTimesView : MonoBehaviour
{
    [Header("-> Components")]
    [SerializeField]private Button _buttonBackToMenu;
    [SerializeField, Tooltip("Mi tiempo en este nivel")] private TextMeshProUGUI _myTimeInLevel;
    [Tooltip("El texto que va a aparecer en pantalla (UI)")]public ViewRecord[] bestTimesView;
    [Tooltip("Color para marcar que record se va a areemplazar"),SerializeField] private Color _colorToRepleaceRecord;
    [Tooltip("Color inicial del texto (UI)")]private Color _defaultColor;

    [Space(10), Header("-> Not Record")]
    [SerializeField, Tooltip("Texto (UI) cuando no se consiga un record")] private TextMeshProUGUI _TMPGoodLuckNextTime;

    private InputYourName _refInputName;

    //Para Tupla
    [Tooltip("Si hay un nuevo record")]private bool _isNewRecord;
    [Tooltip("Indice a reeplazar")]private int _indexToReplace;

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
        (_isNewRecord, _indexToReplace) = GameManager.instance.currentLevel.CheckNewTimeWithTheBestTimes(GameManager.instance.timeInLevel);

        //Si hay un nuevo record, llamo al método para poner mi nombre
        if (_isNewRecord) NewRecord(_indexToReplace);

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

    private void NewRecord(int index)
    {
        _refInputName.inputField.gameObject.SetActive(true);

        for (int i = 0; i < bestTimesView.Length; i++)
        {
            if(i == _indexToReplace)
            {
                bestTimesView[i].nameRecord.color = _colorToRepleaceRecord;
                bestTimesView[i].timeRecord.color = _colorToRepleaceRecord;

                break;
            }
        }
    }

    IEnumerator SwitchColor()
    {
        while (true)
        {

        }
    }


    public void ShowBestTimesRecords()
    {
        if(_refInputName.inputField.gameObject.activeInHierarchy)_refInputName.inputField.gameObject.SetActive(false);

        GameManager.instance.currentLevel.SaveTimeInOrder(GameManager.instance.timeInLevel);

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
