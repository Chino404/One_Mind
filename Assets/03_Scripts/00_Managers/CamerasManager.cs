using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-49)]
public class CamerasManager : MonoBehaviour
{
    public static CamerasManager instance;

    public List<CameraRails> listBongosRailsCamera;
    public CameraRails currentBongoCamera;

    public List<CameraRails> listFranksRailsCamera;
    public CameraRails currentFrankCamera;

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

        //Seteo mi lista.
        if (playerType == CharacterTarget.Bongo)
        {
            list = listBongosRailsCamera;
            currentBongoCamera = newCamera;
        }
        else
        {
            list = listFranksRailsCamera;
            currentFrankCamera = newCamera;
        }

        //Apago las que no correspondan.
        foreach (var camera in list)
        {
            if (camera.NumberCamera != newCamera.NumberCamera)
            {
                //camera.gameObject.SetActive(false);

                camera.gameObject.GetComponent<Camera>().enabled = false;
                camera.gameObject.GetComponent<AudioListener>().enabled = false;

                continue;
            }

            //camera.gameObject.SetActive(true);

            camera.gameObject.GetComponent<Camera>().enabled = true;
            camera.gameObject.GetComponent<AudioListener>().enabled = true;

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
    #endregion
}
