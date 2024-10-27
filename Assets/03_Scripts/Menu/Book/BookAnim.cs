using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BookAnim : MonoBehaviour
{
    [SerializeField] private MenuManager _refMenuManager;
    private Animator _anim;


    [Space(10), SerializeField] private Canvas _pressClickCanvas;
    [SerializeField] private Canvas[] canvas;

    [SerializeField, Tooltip("Delay para que aparezca el canvas")] private float _delay = 1.0f;
    private int currentCanvasIndex = -1;
    private bool onFirstClick = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !onFirstClick)
        {
            //OpenBook();
            OpenBookAnim();
            _pressClickCanvas.gameObject.SetActive(false);
            onFirstClick = true;
        }
    }
    private void OpenBookAnim() => _anim.SetTrigger("OpenBook");
    public void GoToLevelSelectorPageAnim()
    {
        VerifyActiveCanvas();
        _anim.SetTrigger("LevelSelectorPage");
    }

    public void ReturnToMenuAnim()
    {
        VerifyActiveCanvas();
        _anim.SetTrigger("ReturnToMenu");
    }

    public void GoToOptionsOrCredits()
    {
        VerifyActiveCanvas();
        _anim.SetTrigger("LevelSelectorPage");
        _anim.SetTrigger("OptionOrCredits");
    }

    public void OpenBook() => SwitchToCanvas(0); 
    public void GoToLevelSelectorPage() => SwitchToCanvas(1); 
    public void ReturnToMenu() => SwitchToCanvas(0);


    private void VerifyActiveCanvas() => canvas[currentCanvasIndex].gameObject.SetActive(false);



    public void ThirdOpen()
    {
        _anim.SetBool("Open_3", true);
        StartCoroutine(ToggleCanvasAfterDelay(2)); 
    }

    public void CloseSecond()
    {
        _anim.SetBool("Open_2", false);
        StartCoroutine(ToggleCanvasAfterDelay(1, false)); 
    }

    public void CloseThird()
    {
        _anim.SetBool("Open_3", false);
        StartCoroutine(ToggleCanvasAfterDelay(2, false)); 
    }

    /// <summary>
    /// Alternar Canvas después del delay
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isOpen"></param>
    /// <returns></returns>
    private IEnumerator ToggleCanvasAfterDelay(int index, bool isOpen = true) //(canvas a activar/desactivar, bool)
    {
        // Desactivar el canvas actualmente activo, si hay uno
        //if (currentCanvasIndex >= 0 && currentCanvasIndex < canvas.Length && currentCanvasIndex != index)
        //{
        //    canvas[currentCanvasIndex].gameObject.SetActive(false);
        //}

        yield return new WaitForSeconds(_delay); // Espera el tiempo de retraso

        if (isOpen)
        {
            // Desactivar el canvas actualmente activo, si hay uno
            //if (currentCanvasIndex >= 0 && currentCanvasIndex < canvas.Length && currentCanvasIndex != index)
            //{
            //    canvas[currentCanvasIndex].gameObject.SetActive(false);
            //}

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

    private void SwitchToCanvas(int index, bool isOpen = true)
    {
        if (isOpen)
        {
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
