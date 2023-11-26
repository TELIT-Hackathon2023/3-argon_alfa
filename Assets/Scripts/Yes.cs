using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Yes : MonoBehaviour
{
    [SerializeField] private GameObject toTurnOn;

    public IEnumerator TurnOn()
    {
        yield return new WaitForSeconds(5f);
        toTurnOn.SetActive(true);
        gameObject.SetActive(false);
    }
}