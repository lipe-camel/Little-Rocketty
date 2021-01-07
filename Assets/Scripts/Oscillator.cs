using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector; //change in inspector the X, Y and Z to move
    [SerializeField] [Range(0, 1)] float movementFactor; //0 to not moved, 1 to fully moved
    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        Vector3 offset = movementVector * movementFactor; //multiply the movement between 0 and 1
        transform.position = startingPos + offset; //adds movement to the initial position
    }
}
