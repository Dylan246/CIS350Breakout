//libraries we are using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject paddle;
    private bool isBallInPlay;
    private GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetBall();
        gameController = GameObject.FindObjectOfType<GameController>();
    }

    public void ResetBall()
    {
        gameObject.transform.position = paddle.transform.position + new Vector3(0, 0.5f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isBallInPlay = false;
    }

    public void LaunchTheBall()
    {
        if (!isBallInPlay) //If ball is not in play, give it velocity...
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(10, 10); //Getcomponent is a function that gets the component off rigidbody2d 
            isBallInPlay = true;
        }
    }

    public void StopBall()
    {
        gameObject.SetActive(false); //Turn it off 
    }

    // Update is called once per frame
    //Movement, collision detections should go in update
    void Update()
    {
        if (!isBallInPlay) //If ball is not in play follow the paddle
        {
            gameObject.transform.position = paddle.transform.position + new Vector3(0, 0.5f);
        }
        else if (gameObject.transform.position.y < -5) //Less then height, should be -6
        {
            //Lose a life
            gameController.LoseALife();
            //Reset ball
            ResetBall();
        }
    }
}
