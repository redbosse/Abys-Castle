using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arr : MonoBehaviour
{
    [SerializeField]
    private float[] mass = new float[20];

    [SerializeField]
    private List<float> lst = new List<float>();

    private void Reset()
    {
        Debug.Log(mass.Length);
    }
}