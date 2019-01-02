using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();

    }

    void Move()
    {
        Vector3 displacement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            displacement.z++;
        }

        if (Input.GetKey(KeyCode.A))
        {
            displacement.x--;
        }

        if (Input.GetKey(KeyCode.S))
        {
            displacement.z--;
        }

        if (Input.GetKey(KeyCode.D))
        {
            displacement.x++;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            displacement.y++;
        }

        if (Input.GetKey(KeyCode.C))
        {
            displacement.y--;
        }

        transform.position += displacement * Time.deltaTime * 30.0f;
    }
}
