using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTime = 1f;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(float amount)
    {
        textMesh.SetText(amount.ToString());
    }

    private void FixedUpdate()
    {
        float moveYSpeed = 5f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTime -= Time.deltaTime;

        if (disappearTime < 0)
            Destroy(gameObject);
    }
}
