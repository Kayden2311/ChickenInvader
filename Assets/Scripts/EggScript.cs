using System.Collections;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
        
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CheckEggPosition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckEggPosition()
    {
        while (true)
        {
            Vector3 viewPort = Camera.main.WorldToViewportPoint(transform.position);
            if(viewPort.y < 0.05)
            {
                _animator.SetTrigger("Break");
                _rb.bodyType = RigidbodyType2D.Static;
                Destroy(gameObject, 1);
                break;
            }
            yield return null;
        }
    }
}
