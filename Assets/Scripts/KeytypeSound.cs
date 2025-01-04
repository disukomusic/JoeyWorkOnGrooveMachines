using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeytypeSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip keySound;
    [SerializeField] private AudioClip methodCompleteSound;


    public void PlayKeySound()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(keySound);
    }

    public void PlayMethodCompleteSound()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(methodCompleteSound);

    }
}
