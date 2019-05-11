using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBallsCollider : MonoBehaviour
{
    public ScoringSystem scoringSystem;
    public GameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        gameManager.ShowWhichBall(collision.gameObject.tag);

        switch (collision.gameObject.tag)
        {
            case "Solid":
                scoringSystem.BallInPocket(0);
                Destroy(collision.gameObject);
                break;

            case "Striped":
                scoringSystem.BallInPocket(1);
                Destroy(collision.gameObject);
                break;

            case "Black":
                scoringSystem.BlackBallInPocket();
                Destroy(collision.gameObject);
                break;

            case "White":
                scoringSystem.WhiteBallInPocket();
                break;

            default:
                break;
        }
    }
}
