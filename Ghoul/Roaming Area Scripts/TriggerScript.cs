using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    bool _isPlayerPresent;
    public bool IsPlayerPresent {  get { return _isPlayerPresent; } }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) _isPlayerPresent = true;
        else _isPlayerPresent = false;
    }
}
