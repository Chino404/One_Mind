using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockNextLevel : MonoBehaviour
{
    [Tooltip("Si es TRUE, entonces el proximo nivel no me lo desbloquea")]public bool isFinalDemo;
    [Tooltip("Poner numero de build index del nivel siguiente")] public int nextLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>()) UnlockNewLvl();
    }

    /// <summary>
    /// Desbloque el nuevo nivel para jugar
    /// </summary>
    void UnlockNewLvl()
    {
        if (isFinalDemo) return;

        var levelsList = CallJson.instance.refJasonSave.GetSaveData.levels;
        //Debug.Log($"El nivel del indice {CallJson.instance.refJasonSave.GetSaveData.levels[index + 1]} se desbloqueo");

        //Recorro cada nivel hasta encontrar el que coincida con el index del nextlevel.
        foreach (var currentLevel in levelsList)
        {
            //Cuando lo encunetro lo pongo como desbloqueado.
            if (currentLevel.indexLevelJSON /*currentLevel.sceneReferenceSO.BuildIndex*/ == nextLevel)
            {
                currentLevel.isUnlockLevelJSON = true;
                break;
            }
        }

        //Desbloque el proximo nivel. Accedo al diccionario y le cambio su booleano.
        //CallJson.instance.refJasonSave.GetSaveData.UnlocklevelDataDictionary[nextLevel] = true;

    }
}
