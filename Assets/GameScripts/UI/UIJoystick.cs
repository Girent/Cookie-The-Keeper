using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 lastDirection = new Vector3(1f,1f,0);
    private Vector3 currentDirection;
    private Vector2 positionInput;
    private Image joystickBg;
    [SerializeField] private Image joystick;
    [SerializeField] private Animator cameraAnimator;

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

            if (HorizontalInput() != 0 && VerticallInput() != 0)
            {
                currentDirection = new Vector3(HorizontalInput(), VerticallInput(), 0).normalized;
                lastDirection = new Vector3(HorizontalInput(), VerticallInput(), 0).normalized;
                cameraMoveAnimation();
            }
            else
            {
                currentDirection = lastDirection;
            }
        }
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        OnDrag(eventData);
        cameraAnimator.SetBool("IsMove", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        positionInput = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
        cameraAnimator.SetBool("IsMove", false);
    }

    private void cameraMoveAnimation()
    {
        cameraAnimator.SetFloat("Horizontal", positionInput.x);
        cameraAnimator.SetFloat("Vertical", positionInput.y);
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

    public Vector3 GetCurrentDirection()
    {
        return currentDirection;
    }
}
