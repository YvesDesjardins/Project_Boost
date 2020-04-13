using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    float velocity = 3;
    float rotation = 1;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            rigidBody.AddRelativeForce(new Vector3(0, velocity));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 0, rotation));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 0, -rotation));
        }
    }
}
