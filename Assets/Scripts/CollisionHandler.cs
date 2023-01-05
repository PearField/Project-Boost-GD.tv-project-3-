
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    //To stop all sounds when winning of losing
    FMOD.Studio.Bus enginesBus;

    //Reference to make win/lose sounds
    private FMOD.Studio.EventInstance loseExplosion;
    private FMOD.Studio.EventInstance crashSound;
    private FMOD.Studio.EventInstance winSound;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem winParticles;

    bool haveLost = false;
    bool isTransitioning = false;
    bool collisionDisabled = false;

    private void Start()
    {
        enginesBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        
        
    }

    private void Update()
    {
        RespondToDebusKeys();
    }

    private void RespondToDebusKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle collision.
        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (isTransitioning || collisionDisabled)
        {
            CollisionSounds(2);
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("You collided with the Launch Pad");
                break;

            case "Finish":
                StartLandingSequence();
                break;

            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        enginesBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        GetComponent<Movement>().enabled = false;
        Debug.Log("You crashed, try again");
        crashParticles.Play();
        CollisionSounds(0);
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void StartLandingSequence()
    {
        isTransitioning = true;
        enginesBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        GetComponent<Movement>().enabled = false;
        winParticles.Play();
        CollisionSounds(1);
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = ++currentSceneIndex;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void CollisionSounds(int winOrLose)
    {
        //0 = lose
        //1 = win
        //2 = have alreade crashed
        
        if (winOrLose == 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Lose Explosion");
            haveLost = true;
        }
        else if (winOrLose == 1)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Win sound");
        }
        else if (winOrLose == 2 && haveLost == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Crash sound");
        }
    }
}


