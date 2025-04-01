using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    int totalCoins;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            Time.timeScale = 0;

            //Si se paso el nivel normal
            GameManager.instance.currentLevel.isLevelCompleteJSON = true;

            //Si se agarraron todas las monedas
            totalCoins = GameManager.instance.currentLevel.currentCoinsBongoSide + GameManager.instance.currentLevel.currentCoinsFrankSide;

            if (totalCoins == GameManager.instance.totalCoinsInLevel) GameManager.instance.currentLevel.isTakeAllCoinsThisLevel = true;

            //Si se paso en modo cronómetro
            if (GameManager.instance.isChronometerActive)
            {
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

    }

    private void SaveData()
    {
        GameManager.instance.currentLevel.currentCoinsBongoSide += GameManager.instance.currentCollectedCoinsBongo;

        GameManager.instance.currentLevel.currentCoinsFrankSide += GameManager.instance.currentCollectedCoinsFrank;

        foreach (var coin in GameManager.instance.coinsNameList)
        {
            GameManager.instance.currentLevel.TakeMoneyLevelData(coin);
        }
    }
}
