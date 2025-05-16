using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollectibles : MonoBehaviour
{
    [SerializeField] private CharacterTarget _targetCharacter;

    [Space(10)]
    [SerializeField, Range(0,300)] private float _minRotationSpeed;
    [SerializeField, Range(0,300)] private float _maxRotationSpeed;
    private float _rotationSpeed;

    private UICoins uiPoints;
    private LevelData myCurrentLevel;

    private ParticleSystem _particlesCollect;

    private void Awake()
    {
        //Sumo el total de monedas que hay en el nivel
        GameManager.instance.totalCoinsInLevel++;

        myCurrentLevel = GameManager.instance.currentLevel;

        //Si no estoy guardado en el Dict
        if (!myCurrentLevel.dictCoinsJSON.ContainsKey(gameObject.name)) myCurrentLevel.dictCoinsJSON.Add(gameObject.name, false); //Me agrego al diccionario

        //Si ya agarre esta moneda o estoy jugando en modo cronometro, la apago
        else if (myCurrentLevel.dictCoinsJSON[gameObject.name] || GameManager.instance.isChronometerActive) gameObject.SetActive(false);

        if (_targetCharacter == CharacterTarget.Bongo) GameManager.instance.totalCoinsBongoSide++;

        else GameManager.instance.totalCoinsFrankSide++;
    }

    private void Start()
    {
        if (_targetCharacter == CharacterTarget.Bongo) uiPoints = GameManager.instance.uiCoinBongo;

        else uiPoints = GameManager.instance.uiCoinFrank;

        _rotationSpeed = Random.Range(_minRotationSpeed, _maxRotationSpeed);

        _particlesCollect = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, -_rotationSpeed, 0) * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Characters>())
        {
            Debug.Log("agarre coleccionable");

            //Sumo un punto a la UI
            uiPoints.AddPoints(1);

            //myCurrentLevel.TakeMoneyLevelData(gameObject.name);
            GameManager.instance.coinsNameList.Add(gameObject.name);


            if (_targetCharacter == CharacterTarget.Bongo)
            {
                GameManager.instance.currentCollectedCoinsBongo++;
                //myCurrentLevel.currentCoinsBongoSide++;
            }

            else
            {
                GameManager.instance.currentCollectedCoinsFrank++;
                //myCurrentLevel.currentCoinsFrankSide++;
            }

            StartCoroutine(DesactiveCollectible());
        }

    }

    private IEnumerator DesactiveCollectible()
    {
        gameObject.GetComponentInChildren<MeshRenderer>().gameObject.SetActive(false);
        _particlesCollect.Play();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    //private void OnDisable()
    //{
    //    gameObject.GetComponentInChildren<MeshRenderer>().gameObject.SetActive(true);

    //}
}
