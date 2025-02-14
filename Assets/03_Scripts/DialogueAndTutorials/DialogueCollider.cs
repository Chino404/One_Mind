using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueCollider : MonoBehaviour
{
    //public Dialogue dialoguePanel;
    [SerializeField] private GameObject dialogueCanvas;
    //public string[] lines;

    private bool isExecuting;
    public TextMeshProUGUI dialogueText;
    public float textSpeed = 2;
    public string ID;


    private int index;


    private void Start()
    {
        dialogueCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>() && isExecuting == false)
        {
            isExecuting = true;
            dialogueText.text = string.Empty;

            dialogueCanvas.SetActive(true);
            StartDialogue();
        }
    }


    void StartDialogue()
    {
        //dialogueText.text = LocalizationManager.instance.GetTranslate(ID);
        StartCoroutine(WriteDialogue());
    }

    IEnumerator WriteDialogue()
    {
        foreach (char letter in LocalizationManager.instance.GetTranslate(ID).ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    //private void Update()
    //{
    //    if (isExecuting)
    //    {
    //        if (dialogueText.text == lines[index])
    //            StartCoroutine(NextLine(lines));
    //    }
    //}
    #region DialogoSinLocalizationManager
    //public void StartDialogue(string[] lines)
    //{
    //    index = 0;
    //    StartCoroutine(WriteDialogue(lines));
    //}

    //IEnumerator NextLine(string[] lines)
    //{
    //    if (index < lines.Length - 1)
    //    {
    //        index++;
    //        yield return new WaitForSeconds(1f);
    //        dialogueText.text = string.Empty;
    //        StartCoroutine(WriteDialogue(lines));
    //    }

    //    else
    //    {
    //        yield return new WaitForSeconds(1f);
    //        isExecuting = false;
    //        dialogueCanvas.SetActive(false);
    //        gameObject.SetActive(false);
    //    }
    //}

    //IEnumerator WriteDialogue(string[] lines)
    //{
    //    foreach (char letter in lines[index].ToCharArray())
    //    {
    //        dialogueText.text += letter;
    //        yield return new WaitForSeconds(textSpeed);
    //    }
    //}
    #endregion
}
