using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    [SerializeField] private float uiRadius;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private CupSpawner cupSpawner;

    private RectTransform rectTransform;

    private Vector3 cupPosition;

    private void OnEnable()
    {
        cupPosition = cupSpawner.getSpawnPointPosition().position;
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        cameraPosition.z = 0f;
        
        Vector3 direct = (cupPosition - cameraPosition).normalized;

        rectTransform.anchoredPosition = direct * uiRadius;
    }
}
