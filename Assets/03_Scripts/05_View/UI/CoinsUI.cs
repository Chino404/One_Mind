using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsUI : MonoBehaviour
{
    private int points;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        AddPoints(GameManager.instance.currentLevel.totalCoin);
    }
    
    public void AddPoints(int newPoints)
    {       
        points += newPoints;

        textMesh.text = points.ToString();
        textMesh.text = $"{points} / {GameManager.instance.totalCoinsInLevel}";
    }
}
