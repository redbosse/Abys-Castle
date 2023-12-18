using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float damage = 20;

    private Tower parentTower;

    public Tower ParentTower { get => parentTower; set => parentTower = value; }

    private void OnTriggerEnter(Collider other)
    {
        Player enemy = null;

        if ((enemy = other.gameObject.GetComponent<Player>()) is not null)
        {
            enemy.TakeDamage(damage);

            GetComponent<Enemy>().TakeDamage(200);

            return;
        }
        else
        {
            GetComponent<Enemy>().TakeDamage(200);
        }
    }

    private void OnDestroy()
    {
        parentTower.RemoveBullet(this);
    }
}