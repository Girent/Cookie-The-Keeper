using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    private void FootStepsSound()
    {
        audioSource.clip = audioClips[0];
        audioSource.PlayOneShot(audioClips[0]);
    }

    private void AttackSound()
    {
        audioSource.clip = audioClips[1];
        audioSource.PlayOneShot(audioClips[1]);
    }

    public void HitSound()
    {
        audioSource.clip = audioClips[2];
        audioSource.PlayOneShot(audioClips[2]);
    }

    public void WarmUpEndSound()
    {
        audioSource.clip = audioClips[3];
        audioSource.PlayOneShot(audioClips[3]);
    }

    public void EndGameSound()
    {
        audioSource.clip = audioClips[4];
        audioSource.PlayOneShot(audioClips[4]);
    }
}
