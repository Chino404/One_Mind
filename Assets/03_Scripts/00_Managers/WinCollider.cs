using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    int totalCoins;

    [SerializeField] private bool _isWithCinematic;

    public void WinPlayer()
    {

        Time.timeScale = 0;

        //Si se paso el nivel normal
        GameManager.instance.currentLevel.isLevelCompleteJSON = true;

        //Si se paso en modo cronómetro
        if (GameManager.instance.isChronometerActive)
        {
            if (GameManager.instance.secondsInGame && GameManager.instance.secondsInGame.gameObject.activeInHierarchy) GameManager.instance.secondsInGame.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameManager.instance.UIBestTimesInlevel.Show();
        }
        else
        {
            SaveData();

            PauseManager.instance.winCanvas.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {

            if(!_isWithCinematic) WinPlayer();
        }

    }

    private void SaveData()
    {
        #region Coins

        GameManager.instance.currentLevel.coinsObtainedBongoSide = GameManager.instance.currentCollectedCoinsBongo;

        GameManager.instance.currentLevel.coinsObtainedFrankSide = GameManager.instance.currentCollectedCoinsFrank;

        foreach (var coin in GameManager.instance.coinsNameList)
        {
            GameManager.instance.currentLevel.TakeMoneyLevelData(coin);
        }

        //Si se agarraron todas las monedas
        totalCoins = GameManager.instance.currentLevel.coinsObtainedBongoSide + GameManager.instance.currentLevel.coinsObtainedFrankSide;

        if (totalCoins == GameManager.instance.totalCoinsInLevel) GameManager.instance.currentLevel.isTakeAllCoinsThisLevel = true;

        #endregion

        #region Collectable
        //Cambio el booleano del dicccionario por verdadero

        if (GameManager.instance.isTakeCollBongo) CallJson.instance.refJasonSave.ModyfyValueCollectableDict(GameManager.instance.IndexLevel, GameManager.instance.nameCollBongo, true);

        if(GameManager.instance.isTakeCollFrank) CallJson.instance.refJasonSave.ModyfyValueCollectableDict(GameManager.instance.IndexLevel, GameManager.instance.nameCollFrank, true);

        #endregion
    }
}
