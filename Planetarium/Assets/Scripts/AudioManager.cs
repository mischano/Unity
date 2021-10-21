using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField, Range(0.01f, 1f)]
    public float _audioVolume = 0.5f;

    [SerializeField, Range(0.1f, 1f)]
    public float _audioVolumeOffset = 0f;

    private AudioSource audioSource;

    public AudioClip _consumablePop;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound)
    {
        if (sound == "Consumable Pop")
        {
            audioSource.PlayOneShot(_consumablePop, _audioVolume + _audioVolumeOffset);
        }
    }
}
