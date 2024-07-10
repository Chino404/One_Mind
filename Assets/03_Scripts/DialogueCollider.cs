using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollider : MonoBehaviour
{
    public Dialogue dialoguePanel;
    [SerializeField] private GameObject dialogueCanvas;
    public string[] lines;

    private void Start()
    {
        dialogueCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ModelMonkey>())
        {
            dialogueCanvas.SetActive(true);
            dialoguePanel.StartDialogue(lines);
            
        }
    }
}
