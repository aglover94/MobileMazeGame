using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Public Variable
    public GameObject player;

    //Private Variable
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        //Set the offSet value to the value of position minus player position
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Set transform position to the value of player position plus the offset
        transform.position = player.transform.position + offset;
    }
}
