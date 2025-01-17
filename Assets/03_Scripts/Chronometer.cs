using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chronometer : MonoBehaviour
{
    private float _time;
    public TextMeshProUGUI secondsInGame;

    private void Update()
    {
        _time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_time / 60);
        int seconds = Mathf.FloorToInt(_time % 60);
        float fractionalSeconds = _time % 1;
        int decimals = Mathf.FloorToInt(fractionalSeconds * 100);
        
        //secondsInGame.text = string.Format("{0:00:00}:{1:00:00}", minutes, seconds);
        secondsInGame.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds,decimals);
    }
}
