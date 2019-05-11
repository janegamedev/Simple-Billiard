using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float xRotationSpeed = 50.0f;
    public float yRotationSpeed = 5.0f;
    public GameObject ball;
    public GameManager gameManager;

    private void Update()
    {
        if (!Input.GetMouseButton(0) && !gameManager.isPause)
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * xRotationSpeed);
        }
   
        transform.position = ball.transform.position;

        if(gameManager.inMenu)
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }

}
