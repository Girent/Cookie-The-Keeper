using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    
    [SerializeField] private float disappearTime = 1f;
    [SerializeField] private float moveYSpeed = 5f;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void SetPopupText(float amount)
    {
        textMesh.text = amount.ToString();
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTime -= Time.deltaTime;

        if (disappearTime < 0)
            Destroy(gameObject);
    }
}
