using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BookAnim : MonoBehaviour
{
    [SerializeField] private MenuManager _refMenuManager;
    private Animator _anim;
    [SerializeField]private List<AnimationClip> _clips;

    [Space(10), SerializeField] private Canvas[] canvas;

    [SerializeField, Tooltip("Delay para que aparezca el canvas")] private float _delay = 1.0f;
    private int currentCanvasIndex = -1;
    private bool onFirstClick = false;

    private void Awake()
    {
        // Obtener el clip de animación
        //_clips = _anim.runtimeAnimatorController.animationClips;

        //foreach (AnimationClip clip in _clips)
        //{

        //    // Duración en segundos
        //    float duration = clip.length;
        //    Debug.Log($"La duración de la animación {clip.name} es: " + duration + " segundos.");
        //    break;
        //}

        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !onFirstClick)
        {
            OpenBook();
            onFirstClick = true;
        }

        //if (_anim.GetCurrentAnimatorStateInfo(0).IsName("LevelSelectorPage"))
        //{
        //    float animationDuration = _anim.GetCurrentAnimatorStateInfo(0).length;
        //    Debug.Log("Duración de la animación: " + animationDuration + " segundos.");
        //} 

        //if (_anim.GetCurrentAnimatorStateInfo(0))
        //{
        //    float animationDuration = _anim.GetCurrentAnimatorStateInfo(0).length;
        //    Debug.Log("Duración de la animación: " + animationDuration + " segundos.");
        //}
    }

    public void OpenBook()
    {
        //_animBook.SetBool("OpenBook", true);
        _anim.SetTrigger("OpenBook");
        StartCoroutine(ToggleCanvasAfterDelay(0)); 
    }

    public void ReturnToMenu()
    {
        //_animBook.SetBool("ReturnToMenu", false);
        _anim.SetTrigger("ReturnToMenu");
        StartCoroutine(ToggleCanvasAfterDelay(0));
    }

    public void GoToLevelSelectorPage()
    {
        //_animBook.SetBool("LevelSelectorPage", true);
        _anim.SetTrigger("LevelSelectorPage");
        StartCoroutine(ToggleCanvasAfterDelay(1)); 
    }


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
        if (currentCanvasIndex >= 0 && currentCanvasIndex < canvas.Length && currentCanvasIndex != index)
        {
            canvas[currentCanvasIndex].gameObject.SetActive(false);
        }

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
}
