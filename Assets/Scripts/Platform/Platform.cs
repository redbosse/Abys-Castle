using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private Transform upPoint;

    [SerializeField]
    private Transform downPoint;

    [SerializeField]
    private Vector3 upOffset;

    private Transform playerTransform;

    private bool isDown = true;

    [SerializeField]
    private float Speed = 10;

    private Vector3 targetPoint;

    private void Start()
    {
        targetPoint = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, targetPoint) > 0.1f)
        {
            Vector3 direction = (targetPoint - transform.position).normalized;

            if (playerTransform is not null)
            {
                transform.position += direction * Speed * 2 * Time.fixedDeltaTime;
            }
            else
            {
                transform.position += direction * Speed * Time.fixedDeltaTime;
            }

            if (playerTransform is not null)
                playerTransform.position = new Vector3(playerTransform.position.x, transform.position.y + upOffset.y, playerTransform.position.z);
        }
    }

    public void Leverage()
    {
        Debug.Log("Leverage");

        if (isDown)
        {
            targetPoint = upPoint.position;

            isDown = false;
        }
        else
        {
            targetPoint = downPoint.position;
            isDown = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player;

        if ((player = other.GetComponentInChildren<Player>()) is not null)
        {
            playerTransform = player.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player;

        if ((player = other.GetComponentInChildren<Player>()) is not null)
        {
            playerTransform = null;
        }
    }
}