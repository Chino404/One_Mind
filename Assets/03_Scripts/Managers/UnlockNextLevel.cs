using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockNextLevel : MonoBehaviour
{
    [Tooltip("poner numero de build index del nivel siguiente")]public int nextLevel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
            UnlockNewLvl();
    }

    void UnlockNewLvl()
    {
        //if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        //{
        //    PlayerPrefs.SetInt("ReachedIndex", + 1);
        //    PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
        //    PlayerPrefs.Save();

        //}

        var indexNextLevel = nextLevel;
        Debug.Log(indexNextLevel);

        var levels = CallJson.instance.refJasonSave.GetSaveData.levels;

        //Debug.Log($"El nivel del indice {CallJson.instance.refJasonSave.GetSaveData.levels[index + 1]} se desbloqueo");
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].indexLevelJSON == indexNextLevel)
            {
                levels[i].isLevelCompleteJSON = true;
                break;
            }
        }
        //CallJson.instance.refJasonSave.GetSaveData.levels[2].indexLevelJSON;
        CallJson.instance.refJasonSave.GetSaveData.levelDataDictionary[indexNextLevel] = true;

    }
}
