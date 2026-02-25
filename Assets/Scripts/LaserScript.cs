using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private float damageInterval = 0.2f;
    [SerializeField] private AudioClip firesound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Chicken"))
        {
            ChickenScript chicken = collision.GetComponent<ChickenScript>();

            if (chicken != null)
            {
                chicken.ChickenDie();
            }
        }
        if (audioSource != null && firesound != null)
        {
            audioSource.Play();
        }
    }
}