using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemPoints : MonoBehaviour
{
    private int points;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    

    public void AddPoints(int newPoints)
    {
        
        points += newPoints;
        textMesh.text = points.ToString();
    }
}
