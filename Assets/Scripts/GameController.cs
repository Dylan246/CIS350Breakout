using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem; //Needs to be included to work with input actions 
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private InputAction move;
    private InputAction restart;
    private InputAction quit;
    private InputAction launchBall;

    private bool isPaddleMoving;
    [SerializeField] private GameObject paddle;
    [SerializeField] private float paddleSpeed;
    private float moveDirection;
    //Serializing means object comes from the game itself

    [SerializeField] private GameObject brick;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int score;

    [SerializeField] private TMP_Text endGameText;

    private BallController ballController;

    [SerializeField] private TMP_Text livesText;
    private int lives;

    [SerializeField] private TMP_Text restartText;
    [SerializeField] private TMP_Text launchText;

    // Start is called before the first frame update
    void Start()
    {
        DefinePlayerInput();

        //Add code below into a function
        Vector2 brickPos = new Vector2(-9, 5f);
        for (int j = 0; j < 4; j++)
        {
            brickPos.y -= 1;
            brickPos.x = -9;

            for (int i = 0; i < 10; i++)
            {
                brickPos.x += 1.6f; //Same as x = x + 1
                Instantiate(brick, brickPos, Quaternion.identity);
            }
        }

        ballController = GameObject.FindObjectOfType<BallController>();
        lives = 3;
        livesText.text = "Lives: " + lives.ToString();
        scoreText.text = "Score: " + score.ToString();
        restartText.gameObject.SetActive(false);
        
        //Hide game obj
        endGameText.gameObject.SetActive(false);

    }
    public void LoseALife()
    {
        lives--;
        livesText.text = "Lives: " + lives.ToString();

        if(lives == 0)
        {
            endGameText.text = "YOU HAVE FAILED!";
            endGameText.gameObject.SetActive(true); //Basically .visible = true
            ballController.StopBall();
            paddle.SetActive(false);
            restartText.gameObject.SetActive(true);
            launchText.gameObject.SetActive(true);
        }
    }

    public void UpdateScore()
    {
        score += 100;
        scoreText.text = "Score: " + score.ToString(); //Include tostring to be safe

        if( score >= 4000)
        {
            endGameText.text = "YOU WIN!!!";
            endGameText.gameObject.SetActive(true);
            ballController.StopBall();
            paddle.SetActive(false);
            restartText.gameObject.SetActive(true);
            launchText.gameObject.SetActive(true);
        }
    }

    private void DefinePlayerInput()
    {
        //activating the action map
        playerInput.currentActionMap.Enable();

        move = playerInput.currentActionMap.FindAction("MovePaddle");
        restart = playerInput.currentActionMap.FindAction("RestartGame");
        quit = playerInput.currentActionMap.FindAction("QuitGame");
        launchBall = playerInput.currentActionMap.FindAction("LaunchBall");

        //Listeners
        move.started += Move_started;
        move.canceled += Move_canceled;
        restart.started += Restart_started;
        quit.started += Quit_started;
        launchBall.started += LaunchBall_started;


        isPaddleMoving = false;
    }

    private void OnDestroy()
    { 
        move.started -= Move_started;
        move.canceled -= Move_canceled;
        restart.started -= Restart_started;
        quit.started -= Quit_started;
        launchBall.started -= LaunchBall_started;
    }

    private void LaunchBall_started(InputAction.CallbackContext obj)
    {
        ballController.LaunchTheBall();
        launchText.gameObject.SetActive(false);
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        isPaddleMoving = false;
    }
    private void Move_started(InputAction.CallbackContext obj)
    {
        isPaddleMoving = true;
    }

    private void Quit_started(InputAction.CallbackContext obj)
    {
        Application.Quit(); //Will not work in editor
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
        restartText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isPaddleMoving)
        {
            //move the paddle
            paddle.GetComponent<Rigidbody2D>().velocity = new Vector2(paddleSpeed * moveDirection, 0);
        }
        else
        {
            //stop the paddle
            paddle.GetComponent<Rigidbody2D>().velocity = Vector2.zero; //Stops all velocity 
        }
    }


    // Update is called once per frame
    void Update() //depends on framerate 
    {
        if (isPaddleMoving)
        {
            moveDirection = move.ReadValue<float>(); //read value of type float
        }
        //Set ball velocity to be constant 
    }
}
