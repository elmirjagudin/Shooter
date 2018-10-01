using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardWalk : MonoBehaviour
{
    const float MOVE_SPEED = 3.8f;
	void Update ()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            move += Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            move += Vector3.back;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            move += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            move += Vector3.right;
        }

        var t = gameObject.transform;
        move = t.rotation * move * MOVE_SPEED * Time.deltaTime;
        move.y = 0;

        t.position += move;
//        print(move * );
	}
}
