using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //Private Variable
    private GameManager gameManager;

    private void Start()
    {
        //Find the object tagged GameManager and get the GameManager component
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if the other gameObject entering the trigger is tagged Player
        if (other.gameObject.tag == "Player")
        {
            //If it is then call the Win method from GameManager script
            Debug.Log("Goal Reached");
            gameManager.Win();
        }
    }
}
