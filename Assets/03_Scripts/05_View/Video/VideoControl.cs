using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    [Header("-> Video")]
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI txt;
    private float _time = 5;
    private float _currentTime;

    [Space(10), Header("-> Scene Reference")]
    public SceneReferenceSO sceneReferenceSO;
    [Tooltip("Escena de carga asincrónica.")] private int _asyncScene = 1; //Escena a cargar

    private void Awake()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("¡Falta el video!");
            return;
        }

        if (!sceneReferenceSO)
        {
            Debug.LogError("¡Falta la escena a la que se va a ir!");
            return;
        }

        if (txt == null)
        {
            Debug.LogError("¡Falta el texto apra skipear!");
            return;
        }
        else txt.gameObject.SetActive(true);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        videoPlayer.loopPointReached += OnVideoFinished;

        // Reinicia el video desde el inicio
        if (videoPlayer.isPlaying) videoPlayer.Stop(); // Detiene y vuelve al inicio

        videoPlayer.Play(); // Lo vuelve a empezar desde el frame 0

    }

    private void Update()
    {

        if (_currentTime >= _time)
        {
            txt.gameObject.SetActive(false);
        }
        else _currentTime += Time.deltaTime;



        if (Input.GetKeyDown(KeyCode.Escape))
        {

            videoPlayer.Stop(); // Detiene y vuelve al inicio

            SceneManager.LoadSceneAsync(_asyncScene);
            if(sceneReferenceSO) AsyncLoad.sceneReference = sceneReferenceSO;
            else Debug.LogError("¡Falta la escena a la que se va a ir!");

        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            bool isMuted = videoPlayer.GetDirectAudioMute(0); // 0 es el primer track de audio
            videoPlayer.SetDirectAudioMute(0, !isMuted); // Cambia el estado de mute
        }
    }

    // Se llama automáticamente cuando el video termina
    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("¡El video terminó!");

        SceneManager.LoadSceneAsync(_asyncScene);
        if (sceneReferenceSO) AsyncLoad.sceneReference = sceneReferenceSO;
        else Debug.LogError("¡Falta la escena a la que se va a ir!");
    }
}
