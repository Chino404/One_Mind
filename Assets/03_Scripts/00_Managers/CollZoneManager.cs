using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollZoneManager : MonoBehaviour
{
    public static CollZoneManager instance;

    private bool _bongoActiveZone;

    private bool _frankActiveZone;

    [SerializeField] private bool _activeZone;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public void SwitchZone(CharacterTarget player, bool value, int indexScene)
    {
        if(player == CharacterTarget.Bongo) _bongoActiveZone = value;

        else _frankActiveZone = value;


        if (value && !_activeZone)
        {
            SceneManager.LoadSceneAsync(indexScene, LoadSceneMode.Additive);

            _activeZone = true;
        }
        else Debug.Log("No cargue la escena");

        if (!_bongoActiveZone && !_frankActiveZone && _activeZone)
        {
            SceneManager.UnloadSceneAsync(indexScene);

            _activeZone = false;
        }
    }
}
