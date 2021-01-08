using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Physics : MonoBehaviour
{
    Rigidbody rigidBody;

    [SerializeField] float waterMass = 0.3f;
    [SerializeField] float waterDrag = 6f;
    [SerializeField] float atmosphereMass = 1f;
    [SerializeField] float atmosphereDrag = 0.4f;
    [SerializeField] float jellyMass = 0.11f;
    [SerializeField] float jellyDrag = 20f;


    enum SurroundedBy { Atmosphere, ZeroGravity, Water, Jelly }
    SurroundedBy surroundedBy = SurroundedBy.Atmosphere;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Zero Gravity")
        {
            EnableZeroGravity();
        }

        if (collision.gameObject.tag == "Water")
        {
            EnableWaterPhysics();
        }

        if (collision.gameObject.tag == "Jelly")
        {
            EnableJellyPhysics();
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Zero Gravity")
        {
            ReturnToAtmosphere();
        }
        if (collision.gameObject.tag == "Water")
        {
            ReturnToAtmosphere();
        }
        if (collision.gameObject.tag == "Jelly")
        {
            ReturnToAtmosphere();
        }

    }

    //Alternate Physics
    private void EnableZeroGravity()
    {
        surroundedBy = SurroundedBy.ZeroGravity;
        rigidBody.useGravity = false;
    }

    private void EnableWaterPhysics()
    {
        surroundedBy = SurroundedBy.Water;

        rigidBody.mass = waterMass;
        rigidBody.drag = waterDrag;
    }

    private void EnableJellyPhysics()
    {
        surroundedBy = SurroundedBy.Jelly;

        rigidBody.mass = jellyMass;
        rigidBody.drag = jellyDrag;
    }

    private void ReturnToAtmosphere()
    {
        surroundedBy = SurroundedBy.Atmosphere;
        rigidBody.mass = atmosphereMass;
        rigidBody.drag = atmosphereDrag;
        rigidBody.useGravity = true;
    }

}
