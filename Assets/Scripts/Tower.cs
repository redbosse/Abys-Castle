using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private AudioSource shootSFX;

    [SerializeField]
    private float RayRadius = 10;

    [SerializeField]
    private float bulletSpeed = 0.2f;

    [SerializeField]
    private float rate = 0.5f;

    [SerializeField]
    private LayerMask layer;

    [SerializeField]
    private Bullet prefab;

    private List<Transform> transforms = new List<Transform>();

    private Vector3 oldDirection = Vector3.zero;

    private bool isVisible = false;

    private void Start()
    {
        StartCoroutine(upd());
    }

    private void Update()
    {
        if (transforms.Count > 0)
        {
            if (target is not null)
            {
                foreach (var item in transforms)
                {
                    Vector3 direction = (target.position - item.position).normalized;

                    item.position += direction * Time.deltaTime * bulletSpeed;

                    oldDirection = direction;
                }

                if (isVisible)
                    transform.LookAt(target);
            }
            else
            {
                foreach (var item in transforms)
                {
                    item.position += oldDirection * Time.deltaTime * bulletSpeed;
                }
            }
        }
    }

    public void RemoveBullet(Bullet bullet)
    {
        transforms.Remove(bullet.transform);
    }

    private IEnumerator upd()
    {
        while (true)
        {
            if (target is not null)
            {
                if (!Physics.Raycast(spawnPoint.position, (spawnPoint.position - target.position).normalized, RayRadius, layer))
                {
                    var obj = Instantiate(prefab.gameObject) as GameObject;

                    transforms.Add(obj.transform);

                    obj.transform.position = spawnPoint.position;
                    obj.transform.rotation = spawnPoint.rotation;

                    obj.GetComponent<Bullet>().ParentTower = this;

                    shootSFX.pitch = 0.9f + Random.Range(-0.05f, 0.05f);

                    shootSFX.Play();

                    isVisible = true;
                }
                else
                {
                    isVisible = false;
                }
            }

            yield return new WaitForSeconds(rate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player is not null)
        {
            target = player.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player is not null)
        {
            target = null;
        }
    }
}