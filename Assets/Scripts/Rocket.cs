using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rotationForce = 100f;
    [SerializeField] float thrustForce = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip victorySound;
    [SerializeField] float timeUntilLoad = 1f;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

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
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thrustSpeed = (thrustForce * 10) * Time.deltaTime;

        rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
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

    private void StartSucessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(victorySound);
        Invoke("LoadNextScene", timeUntilLoad);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke("RestartScene", timeUntilLoad);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Zero Gravity")
        {
            rigidBody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Zero Gravity")
        {
            rigidBody.useGravity = true;
        }
    }

}
