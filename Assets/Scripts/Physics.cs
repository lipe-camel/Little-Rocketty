using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Physics : MonoBehaviour
{
    Rigidbody rigidBody;

    [Header("Atmosphere Physics")]
    [SerializeField] float atmosphereMass = 1f;
    [SerializeField] float atmosphereDrag = 0.4f;
    [SerializeField] bool atmosphereGravity = true;

    [Header("Water Physics")]
    [SerializeField] float waterMass = 0.3f;
    [SerializeField] float waterDrag = 6f;
    [SerializeField] bool waterGravity = true;

    [Header("Jelly Physics")]
    [SerializeField] float jellyMass = 0.11f;
    [SerializeField] float jellyDrag = 20f;
    [SerializeField] bool jellyGravity = false;

    [Header("Zero Gravity Physics")]
    [SerializeField] float zeroGravityMass = 1f;
    [SerializeField] float zeroGravityDrag = 0.4f;
    [SerializeField] bool zeroGravity = false;


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
        if (collision.gameObject.tag == "Zero Gravity" || collision.gameObject.tag == "Water" || collision.gameObject.tag == "Jelly")
        {
            ReturnToAtmosphere();
        }
    }

    //Alternate Physics
    private void EnableZeroGravity()
    {
        surroundedBy = SurroundedBy.ZeroGravity;

        rigidBody.mass = zeroGravityMass;
        rigidBody.drag = zeroGravityDrag;
        rigidBody.useGravity = zeroGravity;
    }

    private void EnableWaterPhysics()
    {
        surroundedBy = SurroundedBy.Water;

        rigidBody.mass = waterMass;
        rigidBody.drag = waterDrag;
        rigidBody.useGravity = waterGravity;
    }

    private void EnableJellyPhysics()
    {
        surroundedBy = SurroundedBy.Jelly;

        rigidBody.mass = jellyMass;
        rigidBody.drag = jellyDrag;
        rigidBody.useGravity = jellyGravity;
    }

    private void ReturnToAtmosphere()
    {
        surroundedBy = SurroundedBy.Atmosphere;

        rigidBody.mass = atmosphereMass;
        rigidBody.drag = atmosphereDrag;
        rigidBody.useGravity = atmosphereGravity;
    }

}
