using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            Time.timeScale = 0;

            //Si se paso el nivel normal
            GameManager.instance.currentLevel.isLevelCompleteJSON = true;

            //Si se agarraron todas las monedas
            var totalCoins = GameManager.instance.currentLevel.currentCoinsBongoSide + GameManager.instance.currentLevel.currentCoinsFrankSide;

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
                PauseManager.instance.winCanvas.gameObject.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
        }

    }
}
