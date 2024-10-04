using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Book_Anim : MonoBehaviour
{
    public Animator animBook;
    bool onFirstClick = false;

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && onFirstClick != true)
        {
            Open();
            onFirstClick = true;
        }
    }

    public void Open()
    {
        animBook.SetBool("Open_1", true);
    }

    public void SecondOpen()
    {
        animBook.SetBool("Open_2", true);
    }
    public void ThirdOpen()
    {
        animBook.SetBool("Open_2", true);

        animBook.SetBool("Open_3", true);
    }

    public void CloseSecond()
    {
        animBook.SetBool("Open_2", false);
    }

    public void CloseThird()
    {
        animBook.SetBool("Open_3", false);
    }
}
