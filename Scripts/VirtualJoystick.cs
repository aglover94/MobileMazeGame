using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //Private Variables
    private Image backgroundImage;
    private Image joystickImage;
    private Vector3 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        //Get the Image component from the backgroundImage gameObject
        backgroundImage = GetComponent<Image>();
        //Get the 0 index child from the transform, then get the Image component from this child
        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);

            //For joystick placed at bottom left of screen use this
            inputVector = new Vector3(pos.x * 2 - 1, 0, pos.y * 2 - 1);

            /*//For joystick placed at bottom right of screen use this
            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            -1/+1 values represent the position on the screen so minu values in y is bottom of screen, and minus in x would be left of screen etc.
            */

            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //Move joystickImage
            joystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (backgroundImage.rectTransform.sizeDelta.x / 3), inputVector.z * (backgroundImage.rectTransform.sizeDelta.y / 3));

            //Debug.Log(inputVector);
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        //Call OnDrag method passing in ped parameter
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        //Set the inputVector Vector 3 to 0 on X, Y and Z
        inputVector = Vector3.zero;
        //Set the anchoredPosition of joystickImage to 0
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        //Check if the X value of inputVector doesn't equal 0
        if (inputVector.x != 0)
        {
            //If it doesn't then return the value of X in inputVector
            return inputVector.x;
        }
        else
        {
            //Else return the value of the Horizontal axis input
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical()
    {
        //Check if the Z value of inputVector doesn't equal 0
        if (inputVector.z != 0)
        {
            //If it doesn't then return the value of Z in inputVector
            return inputVector.z;
        }
        else
        {
            //Else return the value of the Vertical axis input
            return Input.GetAxis("Vertical");
        }
    }
}