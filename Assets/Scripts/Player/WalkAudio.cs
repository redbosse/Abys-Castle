using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource sourceL;

    [SerializeField]
    private AudioSource sourceR;

    [SerializeField]
    private AudioSource sword;

    [SerializeField]
    private AudioSource sword2;

    private bool swordQuery = false;

    public void right()
    {
        sourceR.Play();
    }

    public void left()
    {
        sourceL.Play();
    }

    public void sw()
    {
        if (swordQuery)
            sword.Play();
        else
            sword2.Play();

        swordQuery = !swordQuery;
    }
}