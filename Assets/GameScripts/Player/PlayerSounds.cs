using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void FootSteps()
    {
        audioSource.Play();
    }
}
