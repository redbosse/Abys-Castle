using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private ParticleSystem bloodParticle;

    [SerializeField]
    private Renderer rend;

    [SerializeField]
    private float HP = 100;

    [SerializeField]
    private float armor = 100;

    [SerializeField]
    private UnityEvent OnDeath;

    private Color prevColor = Color.black;

    private void Start()
    {
        armor = Mathf.Clamp(armor, 0, 100);
    }

    private IEnumerator redTime()
    {
        prevColor = rend.material.color;

        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);

        rend.material.color = prevColor;
    }

    public void TakeDamage(float damage)
    {
        source.Play();

        float dmg = (armor / 100.0f) * damage;

        HP -= dmg;

        Debug.Log($"Take  { dmg}");

        bloodParticle.Play();

        StartCoroutine(redTime());

        if (HP <= 0)
        {
            OnDeath?.Invoke();

            Destroy(gameObject, 1f);
        }
    }
}