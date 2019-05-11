using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCheck : MonoBehaviour
{
    public Ball ball;
    public ScoringSystem scoringSystem;
    public GameObject resetPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "White")
        {
            scoringSystem.WhiteBallInPocket();
        }
        else if(other.gameObject.tag == "Solid"|| other.gameObject.tag == "Striped"|| other.gameObject.tag == "Black")
        {
            other.transform.position = resetPos.transform.position;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
