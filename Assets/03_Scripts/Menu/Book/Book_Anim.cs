using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Book_Anim : MonoBehaviour
{
    public Animator animBook;
    bool onFirstClick = false;
    public Canvas[] canvas;
    public float delay = 1.0f;
    private int currentCanvasIndex = -1; 

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && !onFirstClick)
        {
            Open();
            onFirstClick = true;
        }
    }

    public void Open()
    {
        animBook.SetBool("Open_1", true);
        StartCoroutine(ToggleCanvasAfterDelay(0)); 
    }

    public void SecondOpen()
    {
        animBook.SetBool("Open_2", true);
        StartCoroutine(ToggleCanvasAfterDelay(1)); 
    }

    public void ThirdOpen()
    {
        animBook.SetBool("Open_3", true);
        StartCoroutine(ToggleCanvasAfterDelay(2)); 
    }

    public void CloseSecond()
    {
        animBook.SetBool("Open_2", false);
        StartCoroutine(ToggleCanvasAfterDelay(1, false)); 
    }

    public void CloseThird()
    {
        animBook.SetBool("Open_3", false);
        StartCoroutine(ToggleCanvasAfterDelay(2, false)); 
    }

    private IEnumerator ToggleCanvasAfterDelay(int index, bool isOpen = true)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo de retraso

        if (isOpen)
        {
            // Desactivar el canvas actualmente activo, si hay uno
            if (currentCanvasIndex >= 0 && currentCanvasIndex < canvas.Length && currentCanvasIndex != index)
            {
                canvas[currentCanvasIndex].gameObject.SetActive(false);
            }

            // Activar el nuevo canvas
            if (index >= 0 && index < canvas.Length)
            {
                canvas[index].gameObject.SetActive(true);
                currentCanvasIndex = index; // Actualiza el índice del canvas activo
            }
        }
        else
        {
            // Solo desactivar el canvas actual
            if (currentCanvasIndex == index)
            {
                canvas[index].gameObject.SetActive(false);
                currentCanvasIndex = -1; // Resetea el índice
            }
        }
    }
}
