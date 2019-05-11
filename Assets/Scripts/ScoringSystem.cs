using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public GameManager gameManager;
    public Ball ball;
    public int turn;
    public int temp;

    //balls in this turn
    public bool whiteBall;
    public bool blackBall;
    public int solidBalls;
    public int stripedBalls;
    public int currentPlayerColor;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }
    
    private void Update()
    {
        turn = gameManager.turn;

        if (turn == 0)
            temp = 1;
        else
            temp = 0;
    }

    public void CheckScore()
    {
        if (blackBall)
        {
            if(gameManager.players[turn].playerScore>=7&&!whiteBall)
            {
                gameManager.FinishGame(turn);
                //this player win
            }
            else
            {
                //this player lose
                gameManager.FinishGame(temp);
            }
        }
        else
        {
            if (whiteBall)
            {
                gameManager.ChangeTurn(); //change current player
                ball.ResetBall(); // reset ball
            }
            else
            {
                if (currentPlayerColor == 0)
                {
                    if (solidBalls > 0 && stripedBalls == 0)
                    {
                        Debug.Log("keep this player");
                    }
                    else
                    {
                        gameManager.ChangeTurn();
                    }
                }
                else
                {
                    if (stripedBalls > 0 && solidBalls == 0)
                    {
                        Debug.Log("keep this player");
                    }
                    else
                    {
                        gameManager.ChangeTurn();
                    }
                }
            }
        }

        ResetBallsInThisTurn();
        gameManager.UpdateScore();
    }

    //Check white ball
    public void WhiteBallInPocket()
    {
        whiteBall = true;
    }

    //Check balls
    public void BallInPocket(int ballInPocket) // 0 solid, 1 striped
    {
        if (ballInPocket == 0)
            solidBalls++;
        else
            stripedBalls++;

            if (gameManager.players[turn].hasColor)
            {
                if (ballInPocket == gameManager.players[turn].playerColor)
                    gameManager.players[turn].AddScore();
                else
                    gameManager.players[temp].AddScore();
            }
            else
            {
                gameManager.players[turn].playerColor = ballInPocket;
                gameManager.players[turn].hasColor = true;
                gameManager.players[turn].AddScore();

                int tempColor;
                if (ballInPocket == 0)
                    tempColor = 1;
                else
                    tempColor = 0;

                gameManager.players[temp].playerColor = tempColor;
                gameManager.players[temp].hasColor = true;
            }

        currentPlayerColor = gameManager.players[turn].playerColor;
    }

    ////Check black ball
    public void BlackBallInPocket()
    {
        blackBall=true;
    }

    void ResetBallsInThisTurn()
    {
        whiteBall = false;
        blackBall = false;
        solidBalls = 0;
        stripedBalls = 0;
    }
}
