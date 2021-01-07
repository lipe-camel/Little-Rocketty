using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 0f, 0f); //change in inspector the X, Y and Z to move
    [SerializeField] [Range(0, 1)] float movementFactor; //0 to not moved, 1 to fully moved
    [SerializeField] float period = 2f;

    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        //to do protect against period is zero
        float cycles = Time.time / period; //grows continually from zero

        const float tau = Mathf.PI * 2; // tau ~= 6,28
        float rawSinWave = Mathf.Sin(cycles * tau); //oscilate between -1 and 1

        movementFactor = rawSinWave / 2 + .5f; // oscilate between 0 and 1
        Vector3 offset = movementVector * movementFactor; //multiply the movement between 0 and 1
        transform.position = startingPos + offset; //adds movement to the initial position
    }
}
