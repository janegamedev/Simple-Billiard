using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballForce;
    public bool isPressed = false;
    public bool isInHouse;
    public float speed = 0.001f;
    public float startPosition;
    private float newPosition;
    public int timePast;
    public GameObject startPositionBall;
    public bool isFirst = true;

    public GameManager gameManager;
    public GameObject cameraManager;

    private void Start()
    {
        ResetBall();
    }

    private void Update()
    {
        if (!gameManager.isPause)
        {
            if (Input.GetMouseButtonDown(0) && !isInHouse)
            {
                startPosition = Input.mousePosition.y;
                isPressed = true;
            }

            if (Input.GetMouseButtonUp(0) && isPressed)
            {
                newPosition = Input.mousePosition.y;
                isPressed = false;
                CalculateForce();
                LaunchBall();
            }

            if (isInHouse && Input.GetMouseButtonDown(1))
            {
                int LayerMask = 1 << 9;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask))
                {
                    transform.position = new Vector3(hit.point.x,10.2f,hit.point.z);
                }
            }

            if (isInHouse && Input.GetMouseButtonDown(0))
            {
                isInHouse = false;
                gameManager.HouseTable();
            }
        }
    }

    //Launch ball
    private void LaunchBall()
    {
        Rigidbody rbBall = GetComponent<Rigidbody>();
        rbBall.AddForce(cameraManager.transform.forward * ballForce, ForceMode.Impulse);
        gameManager.isPause = true;

        //isFirst = false;
        gameManager.isHit = true;
        gameManager.isHitting = true;
        gameManager.timeLeft = 5;
    }

    private void CalculateForce()
    {
        float percent = (startPosition - newPosition) / 5;
        if (percent > 100)
            percent = 100;
        if (percent < 0)
            percent = 0;
        ballForce = 4.0f * percent;
    }

    public void ChoosePositionInTheHouse()
    {
        isInHouse = true;
    }

    public void StopBall()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void ResetBall()
    {
        transform.position = startPositionBall.transform.position;

        if (!isFirst)
        {
            ChoosePositionInTheHouse();
            gameManager.HouseTable();
        }
    }
}
