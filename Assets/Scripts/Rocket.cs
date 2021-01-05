using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space pressed");
        }
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A pressed");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("D pressed");
        }
    }

}
