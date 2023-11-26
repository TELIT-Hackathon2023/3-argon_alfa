using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class NPC : MonoBehaviour {
    [SerializeField] private GameObject dialogueWindowObj;
    [SerializeField] private List<string> dialogueStrings;
    [SerializeField] private int optionIndex = -1;
    private PlayerController _player;

    [SerializeField] private bool showAuthorName = false;
    [SerializeField] private GameObject authorNameObj;
    [SerializeField] private string authorName;
    [SerializeField] private TMP_Text authorNameTMP;
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MainPlayer"))
        {
            dialogueWindowObj.SetActive(true);
            dialogueWindowObj.GetComponentInChildren<Dialogue>().SetDialogue(dialogueStrings, optionIndex);
            dialogueWindowObj.GetComponentInChildren<Dialogue>().Author = this;

            if (showAuthorName)
            {
                authorNameObj.SetActive(true);
                authorNameTMP.text = authorName;
            }

            _player = other.gameObject.GetComponent<PlayerController>();
            _player.Stop();
        }
    }

    public void DeactivateDialogue()
    {
        dialogueWindowObj.SetActive(false);
        _player.canMove = true;
        StartCoroutine(GetComponent<Yes>()?.TurnOn());
    }
}