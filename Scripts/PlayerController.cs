using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Public Variables
    public GameObject joystickBackground;
    public float speed;
    

    //Private Variables
    private VirtualJoystick joystick;
    private Rigidbody rb;
    private Vector3 movement;

    // Use this for initialization
    void Start()
    {
        //Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        //Get the VirtualJoystick component from the joystickBackground gameObject
        joystick = joystickBackground.GetComponent<VirtualJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set moveHorizontal and moveVertical values from the relevant methods in the VirtualJoystick script
        float moveHorizontal = joystick.Horizontal();
        float moveVertical = joystick.Vertical();

        //Set movement vector3 using the moveHorizontal and Vertical values
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //Add force to the rigidBody using the movement value multiplied by speed
        rb.AddForce(movement * speed);
    }
}
