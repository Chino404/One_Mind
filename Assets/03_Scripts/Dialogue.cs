using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float textSpeed=0.5f;

    private int index;


    private void Awake()
    {
        dialogueText.text = string.Empty;
    }

    private void Update()
    {
        
    }

    public void StartDialogue(string[]lines)
    {
        index = 0;
        
        StartCoroutine(WriteDialogue(lines));
        
    }

    public void NextLine(string[] lines)
    {
        if (dialogueText.text == lines[index])
        {
            if (index < lines.Length - 1)
            {
                Debug.Log("aaa");
                index++;
                dialogueText.text = string.Empty;
                StartCoroutine(WriteDialogue(lines));
            }
        }
    }

    IEnumerator WriteDialogue(string[]lines)
    {
        
        foreach (char letter  in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
            
        }

        
    }

}
