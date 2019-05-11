using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueMovement : MonoBehaviour
{
    public Ball ball;
    public GameObject cameraManager;
    public GameManager gameManager;


    private void Update()
    {
        transform.position = ball.transform.position;
        transform.rotation = cameraManager.transform.rotation;
        if (!gameManager.GetComponent<GameManager>().isPause)
        {
            if (ball.isPressed)
            {
                float mousePos = Input.mousePosition.y;
                float dif = (ball.startPosition - mousePos);

                transform.GetChild(0).localPosition = new Vector3(0, 2.8f, Mathf.Lerp(-33, -49, Mathf.Clamp(dif, 0f, 1000f) / 1000f));
            }

            if (Input.GetMouseButtonUp(0))
            {
                transform.GetChild(0).localPosition = new Vector3(0, 2.8f, -33);
            }
        }

        if (!gameManager.isHitting && !ball.isInHouse)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
