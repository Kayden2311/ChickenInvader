using System.Collections;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private int damagePerTick = 1;
    [SerializeField] private float damageInterval = 0.2f;
    [SerializeField] private AudioClip firesound;
    private AudioSource audioSource;
    private float lastDamageTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastDamageTime = Time.time;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Damage chickens
        if (collision.CompareTag("Chicken"))
        {
            ChickenScript chicken = collision.GetComponent<ChickenScript>();
            if (chicken != null)
            {
                chicken.ChickenDie();
            }
        }
        
        // Damage boss with interval
        if (collision.CompareTag("Boss"))
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                BossScript boss = collision.GetComponent<BossScript>();
                if (boss != null)
                {
                    boss.PutDamage(damagePerTick);
                    lastDamageTime = Time.time;
                }
            }
        }
        
        if (audioSource != null && firesound != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}