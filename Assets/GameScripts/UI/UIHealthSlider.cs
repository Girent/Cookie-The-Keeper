using UnityEngine;
using UnityEngine.UI;

public class UIHealthSlider : MonoBehaviour
{
    private Image healthSlider;

    private void Start()
    {
        healthSlider = GetComponent<Image>();
    }

    public void UpdateHealthUi(float amount, float maxValue)
    {
        healthSlider.fillAmount = (amount / maxValue * 100) / 100;
    }
}
