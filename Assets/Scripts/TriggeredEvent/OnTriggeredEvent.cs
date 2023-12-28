using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggeredEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent activateEvent;

    [SerializeField]
    private UnityEvent deactivateEvent;

    [SerializeField]
    private bool flag = false;

    private void OnTriggerEnter(Collider other)
    {
        if (flag)
        {
            activateEvent?.Invoke();
        }
        else
        {
            deactivateEvent?.Invoke();
        }

        flag = !flag;

        Debug.Log(other.name);
    }
}