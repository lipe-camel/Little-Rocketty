using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rotationForce = 100f;
    [SerializeField] float thrustForce = 100f;

    [SerializeField] float waterMass = 0.2f;
    [SerializeField] float waterDrag = 20f;
    [SerializeField] float atmosphereMass = 1f;
    [SerializeField] float atmosphereDrag = 0.4f;

    [SerializeField] float loadLevelDelay = 1f;

    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip victorySound;
    //[SerializeField] AudioClip toggleGravity;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem victoryParticle;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

    enum SurroundedBy {  Atmosphere, ZeroGravity, Water, Jelly}
    SurroundedBy surroundedBy = SurroundedBy.Atmosphere;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    //Manage Inputs
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticle.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thrustSpeed = (thrustForce * 10) * Time.deltaTime;

        rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSound);
        }
        if (!mainEngineParticle.isPlaying)
        {
            mainEngineParticle.Play();
        }
    }

    private void RespondToRotateInput()
    {
        float rotationSpeed = rotationForce * Time.deltaTime;

        rigidBody.freezeRotation = true; //take manual control of rotation
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false; //resume physics control of rotation
    }


    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; } //ignore collision information when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSucessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
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
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Zero Gravity")
        {
            DisableZeroGravity();
        }
        if (collision.gameObject.tag == "Water")
        {
            DisableWaterPhysics();
        }
    }

    //Alternate Physics
    private void EnableZeroGravity()
    {
        surroundedBy = SurroundedBy.ZeroGravity;
        rigidBody.useGravity = false;
    }

    private void DisableZeroGravity()
    {
        surroundedBy = SurroundedBy.Atmosphere;
        rigidBody.useGravity = true;
    }

    private void EnableWaterPhysics()
    {
        surroundedBy = SurroundedBy.Water;

        rigidBody.mass = waterMass;
        rigidBody.drag = waterDrag;
    }

    private void DisableWaterPhysics()
    {
        surroundedBy = SurroundedBy.Atmosphere;
        rigidBody.mass = atmosphereMass;
        rigidBody.drag = atmosphereDrag;
    }

    //Scene Management
    private void StartSucessSequence()
    {
        state = State.Transcending;
        StopMainEngine();
        audioSource.PlayOneShot(victorySound);
        victoryParticle.Play();
        Invoke("LoadNextScene", loadLevelDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        StopMainEngine();
        audioSource.PlayOneShot(deathSound, 3f);
        deathParticle.Play();
        Invoke("RestartScene", loadLevelDelay);
    }

    private void StopMainEngine()
    {
        audioSource.Stop();
        mainEngineParticle.Stop();
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
