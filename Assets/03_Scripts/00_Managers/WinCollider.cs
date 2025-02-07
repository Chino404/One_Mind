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

            GameManager.instance.currentLevel.isLevelCompleteJSON = true;

            if (GameManager.instance.chronometerActive) GameManager.instance.currentLevel.isLevelCompleteWithChronometerJSON = true;
            
            PauseManager.instance.winCanvas.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}
