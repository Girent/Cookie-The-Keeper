using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 lastDirection = new Vector3(1f,1f,0);
    private Vector2 positionInput;
    private Image joystickBg;
    [SerializeField] private Image joystick;

    private void Start()
    {
        joystickBg = GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBg.rectTransform, eventData.position, eventData.pressEventCamera, out positionInput))
        {
            positionInput.x = positionInput.x / (joystickBg.rectTransform.sizeDelta.x);
            positionInput.y = positionInput.y / (joystickBg.rectTransform.sizeDelta.y);

            if (positionInput.magnitude > 1.0f)
                positionInput = positionInput.normalized;

            joystick.rectTransform.anchoredPosition = new Vector2(positionInput.x * (joystickBg.rectTransform.sizeDelta.x / 2), positionInput.y * (joystickBg.rectTransform.sizeDelta.y / 2));
        }
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (HorizontalInput() != 0 && VerticallInput() != 0)
            lastDirection = new Vector3(HorizontalInput(), VerticallInput(), 0).normalized;

        positionInput = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float HorizontalInput()
    {
        return positionInput.x;
    }

    public float VerticallInput()
    {
        return positionInput.y;
    }

    public Vector3 GetLastDirection()
    {
        return lastDirection;
    }
}
