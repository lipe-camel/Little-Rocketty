using UnityEngine;

public class Debugger : MonoBehaviour
{
    Rocket rocket;

    private void Start()
    {
        rocket = FindObjectOfType<Rocket>();
    }
    void Update()
    {
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }

    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            rocket.StartSucessSequence();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            rocket.Disablecollisions();
        }
    }

}
