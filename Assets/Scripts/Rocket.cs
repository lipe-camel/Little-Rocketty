using UnityEngine;

[DisallowMultipleComponent]
public class Rocket : MonoBehaviour
{
    //config params
    [Header("Rocket Controls")]
    [SerializeField] float rotationForce = 100f;
    [SerializeField] float thrustForce = 100f;
    [Header("Sounds")]
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip victorySound;
    [Header("Particles")]
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem victoryParticle;

    //state
    bool isTransitioning = false;
    bool collisionsDisabled = false;

    //cached references
    Rigidbody rigidBody;
    AudioSource audioSource;
    SceneLoader sceneLoader;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    void Update()
    {
        if (!isTransitioning)
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
            StopMainEngine();
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

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
            rigidBody.angularVelocity = Vector3.zero;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
            rigidBody.angularVelocity = Vector3.zero;
        }
    }


    public void Disablecollisions() //for debugging purposes
    {
        collisionsDisabled = !collisionsDisabled;
    }


    //Scene Management
    void OnCollisionEnter(Collision collision)
    {
        if(isTransitioning || collisionsDisabled) { return; } //ignore collision information when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                StartSucessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    public void StartSucessSequence()
    {
        isTransitioning = true;
        StopMainEngine();
        audioSource.PlayOneShot(victorySound);
        victoryParticle.Play();
        sceneLoader.LoadNextSceneWithDelay();
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        StopMainEngine();
        audioSource.PlayOneShot(deathSound, 3f);
        deathParticle.Play();
        sceneLoader.RestartSceneWithDelay();
    }

    private void StopMainEngine()
    {
        audioSource.Stop();
        mainEngineParticle.Stop();
    }
}
