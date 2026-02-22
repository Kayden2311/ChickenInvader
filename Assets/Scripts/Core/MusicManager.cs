using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource _audioSource;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0f; // 2D
    }

    public void FadeTo(AudioClip newClip, float duration)
    {
        if (_audioSource.clip == newClip) return;

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeMusic(newClip, duration));
    }

    private IEnumerator FadeMusic(AudioClip newClip, float duration)
    {
        float startVolume = _audioSource.volume;
        float time = 0f;

        // Fade out
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        _audioSource.volume = 0f;
        _audioSource.Stop();

        _audioSource.clip = newClip;
        _audioSource.Play();

        time = 0f;

        // Fade in
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            _audioSource.volume = Mathf.Lerp(0f, startVolume, time / duration);
            yield return null;
        }

        _audioSource.volume = startVolume;
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _audioSource.Stop();
    }
}