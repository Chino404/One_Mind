using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-49)]
public class CamerasManager : MonoBehaviour
{
    public static CamerasManager instance;

    public List<CameraRails> listBongosRailsCamera;
    public CameraRails currentBongoCamera;
    public CameraTracker deathCameraBongo;

    public List<CameraRails> listFranksRailsCamera;
    public CameraRails currentFrankCamera;
    public CameraTracker deathCameraFrank;

    //public delegate void GetCameraDelegate(CameraRails newCamera);
    //public event GetCameraDelegate getCamera;

    private void Awake()
    {
        instance = this;
    }

    #region Cámaras

    /// <summary>
    /// Activo la cámara que me pasaron por parámetro.
    /// </summary>
    /// <param name="newCamera"></param>
    public void ActiveCamera(CameraRails newCamera, CharacterTarget playerType)
    {
        List<CameraRails> list;

        Transform targetPlyaer = default;

        //Seteo mi lista.
        if (playerType == CharacterTarget.Bongo)
        {
            list = listBongosRailsCamera;
            currentBongoCamera = newCamera;

            targetPlyaer = GameManager.instance.modelBongo.transform;
        }
        else
        {
            list = listFranksRailsCamera;
            currentFrankCamera = newCamera;

            targetPlyaer = GameManager.instance.modelFrank.transform;
        }

        //Apago las que no correspondan.
        foreach (var camera in list)
        {
            if (camera.NumberCamera != newCamera.NumberCamera)
            {
                camera.gameObject.SetActive(false);

                continue;
            }

            if (!camera.target) camera.target = targetPlyaer;

            camera.gameObject.SetActive(true);
        }

        //Llamo al evento.
        //getCamera?.Invoke(newCamera);
    }

    /// <summary>
    /// Me da la cámara actual.
    /// </summary>
    /// <param name="playerType"></param>
    /// <returns></returns>
    public CameraRails GetCurrentCamera(CharacterTarget playerType)
    {
        CameraRails camera = null;

        if (playerType == CharacterTarget.Bongo) camera = currentBongoCamera;
        else camera = currentFrankCamera;

        return camera;
    }

    public void DeathCamera(CharacterTarget playerType)
    {
        Debug.LogWarning(playerType);

        //Seteo mi lista.
        List<CameraRails> list = playerType == CharacterTarget.Bongo ? listBongosRailsCamera : listFranksRailsCamera;
        CameraTracker deathCamera = playerType == CharacterTarget.Bongo ? deathCameraBongo : deathCameraFrank;

        foreach (var camera in list)
        {
            camera.gameObject.SetActive(false);
        }

        //deathCamera.gameObject.SetActive(true);
        deathCamera.PlayerDeath();

    }

    public void AliveCamera()
    {
        if (deathCameraBongo.isPlayerDead) deathCameraBongo.PlayerAlive();
        currentBongoCamera.gameObject.SetActive(true);

        if(deathCameraFrank.isPlayerDead) deathCameraFrank.PlayerAlive();
        currentFrankCamera.gameObject.SetActive(true);
    }
    #endregion
}
