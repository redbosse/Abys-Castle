using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBusSystem;

public enum PlayerAnimationEventIndex
{
    attack,
    grab,
    startJump,
    endJump
}

public interface IPlayerAnimationEvent : IGlobalSubscriber
{
    public void Event(PlayerAnimationEventIndex index);
}

public class PlayerAnimationEvent : MonoBehaviour
{
    private int maxSize = 0;

    [SerializeField]
    private string[] enumTable;

    private void OnValidate()
    {
        enumTable = Enum.GetNames(typeof(PlayerAnimationEventIndex));

        for (int i = 0; i < enumTable.Length; i++)
        {
            enumTable[i] = enumTable[i] + " " + i;
        }
    }

    private void Start()
    {
        maxSize = Enum.GetNames(typeof(PlayerAnimationEventIndex)).Length;
    }

    public void EV(int event_index)
    {
        if (event_index > maxSize)
        {
            throw new Exception("Event index large maxEventSize");
        }

        EventBus.RaiseEvent<IPlayerAnimationEvent>(h => h.Event((PlayerAnimationEventIndex)event_index));
    }
}