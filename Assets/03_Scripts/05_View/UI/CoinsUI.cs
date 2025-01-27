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

        var level = CallJson.instance.refJasonSave.GetSaveData.levels;

        AddPoints(level[0].totalCoin);
    }
    
    public void AddPoints(int newPoints)
    {       
        points += newPoints;
        textMesh.text = points.ToString();
    }
}
