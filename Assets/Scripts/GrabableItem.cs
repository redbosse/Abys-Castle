using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableItem : MonoBehaviour
{
    public enum TypeOfGrabbing
    {
        item,
        sword,
        leverage
    }

    [SerializeField]
    private TypeOfGrabbing type;

    [SerializeField]
    private string title;

    [SerializeField]
    private bool isDestroy = false;

    [SerializeField]
    private GameObject prefab;

    public TypeOfGrabbing Type { get => type; set => type = value; }
    public bool IsDestroy { get => isDestroy; set => isDestroy = value; }
    public string Title { get => title; set => title = value; }

    public virtual GameObject Grab()
    {
        var obj = Instantiate(prefab);

        return obj;
    }
}