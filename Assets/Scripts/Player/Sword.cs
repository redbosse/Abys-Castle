using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sword : MonoBehaviour
{
    [System.Serializable]
    public struct ArrayParticle
    {
        [SerializeField]
        private ParticleSystem[] attackParticle;

        public ParticleSystem[] AttackParticle { get => attackParticle; set => attackParticle = value; }

        public void Play()
        {
            foreach (var item in attackParticle)
            {
                item.Play();
            }
        }
    }

    [SerializeField]
    private AudioSource source;

    public AudioClip[] clips;

    [SerializeField]
    private ParticleSystem particle;

    [SerializeField]
    private float[] mass;

    [SerializeField]
    private ArrayParticle attackParticle;

    [SerializeField]
    private float damage = 50;

    private UnityAction stoppingAttack = delegate { };
    private bool isActivateSword = false;

    public bool IsActivateSword { get => isActivateSword; set => isActivateSword = value; }
    public UnityAction StoppingAttack { get => stoppingAttack; set => stoppingAttack = value; }

    public void StartAttack()
    {
        attackParticle.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsActivateSword)
            return;

        source.clip = clips[Random.Range(0, clips.Length)];

        source.Play();

        particle.Play();

        Enemy enemy = null;

        if ((enemy = other.gameObject.GetComponent<Enemy>()) is not null)
        {
            enemy.TakeDamage(damage);

            return;
        }
        else
        {
            stoppingAttack?.Invoke();
        }
    }
}