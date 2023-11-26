using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NPC : MonoBehaviour {
    [SerializeField] private GameObject dialogueWindowObj;
    [SerializeField] private List<string> dialogueStrings;
    private PlayerController _player;
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MainPlayer"))
        {
            dialogueWindowObj.SetActive(true);
            dialogueWindowObj.GetComponentInChildren<Dialogue>().SetDialogue(dialogueStrings);
            dialogueWindowObj.GetComponentInChildren<Dialogue>().Author = this;

            _player = other.gameObject.GetComponent<PlayerController>();
            _player.Stop();
        }
    }

    public void DeactivateDialogue()
    {
        dialogueWindowObj.SetActive(false);
        _player.canMove = true;
    }
}