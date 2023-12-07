using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMoveScript : MonoBehaviour
{
    public float speed = 0.01f;
    public float distance = 10;

    private void FixedUpdate()
    {
        if(transform.position.x > distance)
        {
            speed = speed * -1;
        }
        if(transform.position.x < 0)
        {
            speed = speed * -1;
        }
        transform.position = new Vector3(transform.position.x + speed, 0);
    }
}
