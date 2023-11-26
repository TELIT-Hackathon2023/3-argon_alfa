using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueTMP;
    [SerializeField] private GameObject optionsObj; 
    private List<string> _dialogueStrings;
    private int _optionIndex;
    [SerializeField] private float delayBetweenCharacters = 0.1f;
    private int currentDialogueIndex = 0;
    [HideInInspector] public NPC Author;

    [SerializeField] private GameObject newQuest; 
    private Coroutine _showDialogueCoroutine;
    private void Update()
    {
        if (Input.anyKeyDown && currentDialogueIndex != _optionIndex)
        {
            NextDialogue();
        }
    }

    private void NextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex >= _dialogueStrings.Count)
        {
            EndDialogue();
            return;
        }
        if (_showDialogueCoroutine != null) StopCoroutine(_showDialogueCoroutine);
        _showDialogueCoroutine = StartCoroutine(ShowDialogue());

        if (currentDialogueIndex == _optionIndex)
            optionsObj.SetActive(true);
        else
            optionsObj.SetActive(false);
    }

    private IEnumerator ShowDialogue()
    {
        dialogueTMP.text = "";
        foreach (char c in _dialogueStrings[currentDialogueIndex])
        {
            dialogueTMP.text += c;
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
    }

    public void SetDialogue(List<string> strings, int optionIndex = -1)
    {
        _dialogueStrings = new List<string>(strings);
        currentDialogueIndex = -1;
        _optionIndex = optionIndex;
        NextDialogue();
    }

    public void AcceptQuest()
    {
        newQuest.SetActive(true);
        NextDialogue();
    }

    public void DenyQuest()
    {
        NextDialogue();
    }

    private void EndDialogue()
    {
        optionsObj.SetActive(false);
        Author.DeactivateDialogue();
    }
}