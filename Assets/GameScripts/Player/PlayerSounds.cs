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
        audioSource.Play();
    }

    private void AttackSound()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }
}
