using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFinalDemo : MonoBehaviour
{
    [Tooltip("Escena de carga asincr�nica.")] private int _asyncScene = 1;

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(_asyncScene);
        AsyncLoad.sceneNumber = 0;
    }
}
