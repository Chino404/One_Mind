using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chronometer : MonoBehaviour
{
    [Header("-> Reference")]
    public GameObject secondsInGame;

    private TextMeshProUGUI _txtSecondsInGame;

    [Space(5), Header("-> Change Scale")]
    [SerializeField, Tooltip("Duración del tiempo de modificacion de la escala") , Range(0,0.5f)]public float _duration = 0.15f; // Duración del efecto
    [SerializeField, Tooltip("La escala a la que va a transicionar")] private Vector3 _newScale;
    private Vector3 _originalScale;

    private enum TypeWarning { None, Warning, LostStar}
    private TypeWarning _myTypeWarning;

    private bool _isChangeWarningScale;


    private float _time;

    [Space(10), Header("-> Colors Warning")]
    [SerializeField] private Color _warningColor;
    [SerializeField] private Color _lostStarColor;


    private void Awake()
    {
        bool playWithTimer = CallJson.instance.refJasonSave.GetSaveData.playWithTimer;

        _myTypeWarning = TypeWarning.None;

        gameObject.SetActive(playWithTimer);
    }

    private void Start()
    {
        secondsInGame.SetActive(true);

        _txtSecondsInGame = secondsInGame.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _originalScale = _txtSecondsInGame.transform.localScale;

        //Guardo la referencia en el GameManager
        GameManager.instance.secondsInGame = secondsInGame;

        CallJson.instance.refJasonSave.GetSaveData.playWithTimer = false;
        CallJson.instance.refJasonSave.SaveJSON();

    }

    private void Update()
    {
        //if(Time.timeScale == 0)
        //{
        //    secondsInGame.gameObject.SetActive(false);
        //    return;
        //}

        _time += Time.deltaTime;

        GameManager.instance.timeInLevel.TimeInSeconds = _time;

        int minutes = Mathf.FloorToInt(_time / 60);
        int seconds = Mathf.FloorToInt(_time % 60);
        float fractionalSeconds = _time % 1;
        int decimals = Mathf.FloorToInt(fractionalSeconds * 100);

        if (_time > GameManager.instance.BestTimesInLevel[0].TimeInSeconds && _myTypeWarning == TypeWarning.Warning)
        {
            _myTypeWarning = TypeWarning.LostStar;
            _txtSecondsInGame.color = _lostStarColor;

            StartCoroutine(WarningScale());
        }
        else if (_time > GameManager.instance.BestTimesInLevel[0].TimeInSeconds * 0.75f && _myTypeWarning == TypeWarning.None)
        {
            _myTypeWarning = TypeWarning.Warning;
            _txtSecondsInGame.color = _warningColor;

            StartCoroutine(WarningScale());
        }
        
        //secondsInGame.text = string.Format("{0:00:00}:{1:00:00}", minutes, seconds);
        _txtSecondsInGame.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds,decimals);
    }

    IEnumerator WarningScale()
    {
        float elapsedTime = 0f;
        //Vector3 targetScale = _originalScale * scaleFactor;

        // Escalar hacia arriba
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            _txtSecondsInGame.transform.localScale = Vector3.Lerp(_originalScale, _newScale, elapsedTime / _duration);

            yield return null;
        }

        //yield return new WaitForSeconds(0.25f); // Pequeña pausa antes de volver a la escala original

        elapsedTime = 0f;

        // Regresar a la escala original
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            _txtSecondsInGame.transform.localScale = Vector3.Lerp(_newScale, _originalScale, elapsedTime / _duration);

            yield return null;
        }
    }
}
