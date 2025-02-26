using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevelSelector : MonoBehaviour
{
    private Button _button;
    private LevelData _currentLevel;

    [Header("-> General Config, Button")]
    [SerializeField] private string _levelName;
    [Tooltip("Index del nivel")] public int indexLevel;
    [SerializeField] private int _totalCoins = 0;

    [Space(10)]
    [SerializeField] private Image _lockedImage;
    [SerializeField] private GameObject _ui;
    [SerializeField, Tooltip("Tiempo de transición de escala"),Range(0.1f, 1f)] private float _timeLerpScale;
    private Vector3 _iniScale = new Vector3(0,0,0);
    private Vector3 _endScale = new Vector3(1.5f, 1.5f, 1.5f);

    [Space(7), Header("-> Config. Star Image")]
    public Image[] starImage = new Image[3];
    [SerializeField] private Color _activedStarColor;

    [Space(7), Header("-> Config. Text")]
    [SerializeField] private TextMeshProUGUI _txmpLvelName;
    [SerializeField] private TextMeshProUGUI _txmpTotalCoins;
    [SerializeField] private TextMeshProUGUI _txmpMyBestTime;


    private void Awake()
    {
        _button = GetComponent<Button>();

        _currentLevel = CallJson.instance.refJasonSave.GetCurrentLevel(indexLevel);

        //Si tengo algo escrito en el nombre del nivel
        if (_levelName != string.Empty && _txmpLvelName != null) _txmpLvelName.text = _levelName;

        //El total de las orbes
        if (_txmpTotalCoins != null) _txmpTotalCoins.text = $"{_currentLevel.currentCoinsBongoSide + _currentLevel.currentCoinsFrankSide} / {_totalCoins}";

        if (_txmpMyBestTime != null && _currentLevel.myBestTimeRecord.isBusy && _currentLevel.isLevelCompleteJSON)
        {
            _txmpMyBestTime.text = _currentLevel.myBestTimeRecord.txtTimeInMinutes;
            _txmpMyBestTime.gameObject.SetActive(true);
        }
        else _txmpMyBestTime.gameObject.SetActive(false);

        _ui.gameObject.SetActive(false);

        if (!_currentLevel.isUnlockLevelJSON) _button.interactable = false;
        else
        {
            _button.interactable = true;
            _lockedImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Cuando el mouse esta sobre el botón
    /// </summary>
    public void PointEnterFUnc()
    {
        if (!_button.interactable) return;

        _ui.gameObject.SetActive(true);

        StartCoroutine(SwitchScale());

        //foreach (var image in starImage)
        //{
        //    image.gameObject.SetActive(true);
        //}

        if (_currentLevel.isLevelCompleteJSON) starImage[0].color = _activedStarColor;

        if (_currentLevel.isTakeAllCoinsThisLevel) starImage[1].color = _activedStarColor;

        if (_currentLevel.isLevelCompleteWithChronometerJSON) starImage[2].color = _activedStarColor;
    }

    IEnumerator SwitchScale()
    {
        float currentTime = 0;

        while (currentTime < _timeLerpScale)
        {
            currentTime += Time.deltaTime;

            float progress = currentTime / _timeLerpScale;

            _ui.transform.localScale = Vector3.Lerp(_iniScale, _endScale, progress);

            yield return null;
        }

        _ui.transform.localScale = _endScale;
    }

    /// <summary>
    /// Cuando hago click en el botón
    /// </summary>
    public void PointClickFunc()
    {
        if (!_button.interactable) return;

        MenuManager.Instance.PlayGame(indexLevel, _currentLevel.isLevelCompleteJSON);
    }

    /// <summary>
    /// Cuando el mouse sale del botón
    /// </summary>
    public void PointExitFunc()
    {
        if (!_button.interactable) return;

        _ui.gameObject.SetActive(false);

        _ui.transform.localScale = _iniScale;

    }
}
