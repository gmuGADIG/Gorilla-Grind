using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalAudioSource : MonoBehaviour
{
    Sound sound;
    AudioSource audioSource;
    Transform followTarget;
    float delay = 0f;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
		audioSource.maxDistance = 15;
    }

    public void SetSound(Sound sound)
    {
        this.sound = sound;
        audioSource.volume = this.sound.volume * (1f + UnityEngine.Random.Range(-this.sound.volumeVariance / 2f, this.sound.volumeVariance / 2f));
        audioSource.pitch = this.sound.pitch * (1f + UnityEngine.Random.Range(-this.sound.pitchVariance / 2f, this.sound.pitchVariance / 2f));
        audioSource.minDistance = this.sound.minimunDistance;
        audioSource.clip = this.sound.GetRandomAudioClip();
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    public void SetDelay(float delay)
    {
        this.delay = delay;
    }

    public void Play()
    {
        audioSource.PlayDelayed(delay);
    }

    public void Stop()
    {
        audioSource.Stop();
        followTarget = null;
        delay = 0f;
        sound = null;
    }

    void Update()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.position;
        }
    }
}
