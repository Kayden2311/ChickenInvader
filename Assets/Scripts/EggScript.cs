using System.Collections;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _fallSpeed = 5f;

    private bool _isBreaking = false;

    private void Awake()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Reset trạng thái khi spawn (quan trọng nếu dùng Object Pool)
        _isBreaking = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (!_isBreaking)
        {
            // Ép vận tốc cố định xuống dưới
            _rb.linearVelocity = Vector2.down * _fallSpeed;
        }
    }

    private void Start()
    {
        StartCoroutine(CheckEggPosition());
    }

    private IEnumerator CheckEggPosition()
    {
        while (true)
        {
            Vector3 viewPort = Camera.main.WorldToViewportPoint(transform.position);

            if (viewPort.y < 0.05f && !_isBreaking)
            {
                BreakEgg();
                yield break;
            }

            yield return null;
        }
    }

    private void BreakEgg()
    {
        _isBreaking = true;

        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Static;

        _animator.SetTrigger("Break");

        Destroy(gameObject, 1f);
    }
}