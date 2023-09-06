using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{

    //Reference to game controller
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
    }

    //When something collides with the brick, this function is called
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Ball")
        {
            gameController = GameObject.FindObjectOfType<GameController>(); //Added to fix bug 
            gameController.UpdateScore(); //Calls UpdateScore in game controller to update score
            Destroy(gameObject);
        }
    }
}
