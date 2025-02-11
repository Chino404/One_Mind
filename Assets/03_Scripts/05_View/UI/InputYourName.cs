using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputYourName : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.characterLimit = 15; // Limita a 15 caracteres
        inputField.onValueChanged.AddListener(ValidarTexto);
    }

    void ValidarTexto(string texto)
    {
        // Elimina espacios y actualiza el campo
        inputField.text = texto.Replace(" ", "");
    }

    public void GuardarNombre()
    {
        string nombre = inputField.text;
        GameManager.instance.timeInLevel.name = nombre;
        //Debug.Log("Nombre guardado: " + nombre);
    }
}
