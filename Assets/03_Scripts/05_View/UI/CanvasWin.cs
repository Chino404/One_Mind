using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWin : MonoBehaviour
{
    [Header("-> Positions")]
    [SerializeField] private Vector2 _iniPos;
    [SerializeField] private Vector2 _endPos;
    [SerializeField, Range(0.1f, 1)] private float _timeToShow;
    [Tooltip("Es para la posicion en el canvas")] private RectTransform _rectTransform;

    [Space(7), Header("-> Config. Star Image")]
    public Image[] starImage = new Image[3];
    [SerializeField] private Color _activedStarColor;

    [Space(7), Header("-> Ui COins")]
    [SerializeField] private UICoins _uiBongosCoins;
    private int _currentBongosCoins;
    [SerializeField] private UICoins _uiFranksCoins;
    private int _currentFranksCoins;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _rectTransform.anchoredPosition = _iniPos;

        _currentBongosCoins = GameManager.instance.currentCollectedCoinsBongo;

        _currentFranksCoins = GameManager.instance.currentCollectedCoinsFrank;

    }

    /// <summary>
    /// Mostrar el canvas de victoria
    /// </summary>
    public void ShowCanvas()
    {
        StartCoroutine(Show());
    }

    #region Show Canvas

    IEnumerator Show()
    {
        _rectTransform.anchoredPosition = _iniPos;

        // Movimiento suave hacia endPos con rebote
        yield return StartCoroutine(MoveOverTime(_rectTransform, _iniPos, _endPos, _timeToShow));

        // Mini rebote (como lo hacías antes)
        yield return StartCoroutine(MoveOverTime(_rectTransform, _endPos, new Vector2(0, 143), 0.18f));
        yield return new WaitForSecondsRealtime(0.05f);
        yield return StartCoroutine(MoveOverTime(_rectTransform, new Vector2(0, 143), _endPos, 0.13f));

        //Espero antes de hacer aparecer las estrellas
        yield return new WaitForSecondsRealtime(0.5f);
        StartCoroutine(ActiveStars());
    }

    IEnumerator MoveOverTime(RectTransform rect, Vector2 from, Vector2 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            rect.anchoredPosition = Vector2.LerpUnclamped(from, to, t);
            yield return null;
        }
        rect.anchoredPosition = to;
    }

    #endregion

    #region Scale Stars image

    IEnumerator ActiveStars()
    {
        float time = 0.17f;

        //Nivel completado
        yield return StartCoroutine(ActivateStars(0, time, GameManager.instance.currentLevel.isLevelCompleteJSON));

        //Cantidad de monedas recolectadas
        yield return StartCoroutine(AddPointsUI());
        yield return StartCoroutine(ActivateStars(1, time, GameManager.instance.currentLevel.isTakeAllCoinsThisLevel));

        //Nivel pasado con cronómetro
        yield return StartCoroutine(ActivateStars(2, time, GameManager.instance.currentLevel.isLevelCompleteWithChronometerJSON));
    }

    IEnumerator ActivateStars(int index, float delay, bool condition)
    {
        //yield return new WaitForSecondsRealtime(delay);

        if (condition)
        {
            starImage[index].color = _activedStarColor;

            yield return StartCoroutine(ScaleOverTime(starImage[index].transform, 1.25f, 1.6f, delay));

            yield return StartCoroutine(ScaleOverTime(starImage[index].transform, 1.6f, 1.25f, delay));
        }
    }


    IEnumerator ScaleOverTime(Transform target, float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            float scale = Mathf.Lerp(from, to, t);
            target.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }

    #endregion

    #region Add points in UICoins

    IEnumerator AddPointsUI()
    {
        bool adds = true;

        while (adds == true)
        {

            if(_currentBongosCoins > 0)
            {
                _uiBongosCoins.AddPoints(1);
                _currentBongosCoins--;
            }

            if(_currentFranksCoins > 0)
            {
                _uiFranksCoins.AddPoints(1);
                _currentFranksCoins--;
            }

            if(_currentBongosCoins <= 0 && _currentFranksCoins <= 0) adds = false;

            yield return new WaitForSecondsRealtime(0.15f);
        }

        yield return new WaitForSecondsRealtime(0.25f);
    }

    #endregion
}
