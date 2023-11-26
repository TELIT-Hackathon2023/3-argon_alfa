using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueTMP;
    private List<string> _dialogueStrings;
    [SerializeField] private float delayBetweenCharacters = 0.1f;
    private int currentDialogueIndex = 0;
    [HideInInspector] public NPC Author;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            currentDialogueIndex++;
            if (currentDialogueIndex >= _dialogueStrings.Count)
            {
                EndDialogue();
                return;
            }
            dialogueTMP.text = _dialogueStrings[currentDialogueIndex];
        }
    }

    public void SetDialogue(List<string> strings)
    {
        _dialogueStrings = new List<string>(strings);
        currentDialogueIndex = 0;
        dialogueTMP.text = _dialogueStrings[currentDialogueIndex];
    }

    private void EndDialogue()
    {
        Author.DeactivateDialogue();
    }
}