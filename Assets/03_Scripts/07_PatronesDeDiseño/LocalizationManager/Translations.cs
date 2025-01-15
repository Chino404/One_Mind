using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Translations : MonoBehaviour
{
    public string ID;
    public TextMeshProUGUI textUI;
    private void Start()
    {
        textUI.text = LocalizationManager.instance.GetTranslate(ID);
    }
}
