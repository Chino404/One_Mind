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

    public void GoToOptionsPageAnim()
    {
        VerifyActiveCanvas();
        _anim.SetTrigger("OptionOrCredits");
    }

    public void GoToCreditsAnim()
    {
        VerifyActiveCanvas();
        _anim.SetTrigger("OpenCredits");
    }

    public void ReturnToMenuAnim()
    {
        VerifyActiveCanvas();
        _anim.SetTrigger("ReturnToMenu");
    }

    public void ReturnToLevelSelectorAnim()
    {
        VerifyActiveCanvas();
        _anim.SetTrigger("ReturnToMenu");
    }


    public void OpenBook() => SwitchToCanvas(0); 
    public void GoToLevelSelectorPage() => SwitchToCanvas(1); 
    public void GoToOptionsPage() => SwitchToCanvas(2);
    public void GoToCredtisPage() => SwitchToCanvas(3);
    public void ReturnToMenu() => SwitchToCanvas(0);

    /// <summary>
    /// Verifico si hay un Canvas activado antes
    /// </summary>
    private void VerifyActiveCanvas() => canvas[currentCanvasIndex].gameObject.SetActive(false);


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
