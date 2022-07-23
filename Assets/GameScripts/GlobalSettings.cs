using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 45;
    }
}
