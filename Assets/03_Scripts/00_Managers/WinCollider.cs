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

            //Recorro cada nivel hasta encontrar el que coincida con el index actual
            foreach (var currentLevel in CallJson.instance.refJasonSave.GetSaveData.levels)
            {
                if (currentLevel.indexLevelJSON == GameManager.instance.IndexLevel)
                {
                    currentLevel.isLevelCompleteJSON = true; //Y lo marco como completado
                    break;
                }
            }
            
            PauseManager.instance.winCanvas.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}
