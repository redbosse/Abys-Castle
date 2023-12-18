using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GrabZone : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject grabbingIcon;

    [SerializeField]
    private TextMeshProUGUI text;

    private HashSet<GrabableItem> grabs = new HashSet<GrabableItem>();

    [SerializeField]
    private UnityAction<Sword> grabSword = delegate { };

    public UnityAction<Sword> GrabSword { get => grabSword; set => grabSword = value; }

    public int GrabbingCount()
    {
        return grabs.Count;
    }

    public void StartGrab()
    {
        if (grabs.Count == 0) return;

        foreach (var item in grabs)
        {
            Debug.Log($"{item.Title} is Grabbing");

            grabs.Remove(item);

            var obj = item.Grab();

            if (item.Type == GrabableItem.TypeOfGrabbing.sword)
            {
                var sword = obj.GetComponentInChildren<Sword>();

                GrabSword?.Invoke(sword);
            }

            if (item.IsDestroy)
                Destroy(item.gameObject);

            ReinitText();
            grabbingIcon.SetActive(false);

            return;
        }
    }

    public void Grab()
    {
        if (grabs.Count == 0) return;

        foreach (var item in grabs)
        {
            Debug.Log($"{item.name} is Grabbing");

            ReinitText();
            grabbingIcon.SetActive(false);

            return;
        }
    }

    private void ReinitText()
    {
        string str = "";

        foreach (var itm in grabs)
        {
            str += itm.Title + "\n";
        }

        text.text = str;
    }

    private void OnTriggerEnter(Collider other)
    {
        GrabableItem item;

        if ((item = other.GetComponent<GrabableItem>()) is not null)
        {
            grabbingIcon.SetActive(true);

            grabs.Add(item);
        }

        ReinitText();
    }

    private void OnTriggerExit(Collider other)
    {
        GrabableItem item;

        if ((item = other.GetComponent<GrabableItem>()) is not null)
        {
            grabbingIcon.SetActive(false);
            grabs.Remove(item);
        }

        ReinitText();
    }
}