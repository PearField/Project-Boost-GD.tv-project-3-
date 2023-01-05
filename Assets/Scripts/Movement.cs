using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotateSpeed = 1f;
    
    private FMOD.Studio.EventInstance mainEngineSound;
    private FMOD.Studio.EventInstance rotationEngineSound;


    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftEngineParticles;
    [SerializeField] ParticleSystem rightEngineParticles;


    AudioSource audioSource;
    Rigidbody myRigidbody;

    


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        
        audioSource = GetComponent<AudioSource>();

        mainEngineSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Main booster");
        rotationEngineSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Side boosters");
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    //Processes input to make the spacecraft boost
    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopThrusting();
        }
    }

    //Processes input to make spacecraft rotate
    void ProcessRotation()
    {
        // Starts rotation engine sound
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(rotationEngineSound, transform, GetComponent<Rigidbody>());
            rotationEngineSound.start();
        }

        // Rotation 
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
        
    }

    void StartThrusting()
    {
        myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
        
        // Plays engine sound once when first pressing down space to boost.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(mainEngineSound, transform, GetComponent<Rigidbody>());

            mainEngineSound.start();
        }
        
    }

    void StopThrusting()
    {
        // Stops particle effects and sends engine sound to last point in sound event, effectively ending the sound.
        mainEngineParticles.Stop();
        mainEngineSound.keyOff();
    }

    void RotateRight()
    {
        Rotate(-rotateSpeed);
        if (!leftEngineParticles.isPlaying)
        {
            leftEngineParticles.Play();
        }
    }

    void RotateLeft()
    {
        Rotate(rotateSpeed);
        if (!rightEngineParticles.isPlaying)
        {
            rightEngineParticles.Play();
        }
    }

    private void StopRotating()
    {
        leftEngineParticles.Stop();
        rightEngineParticles.Stop();
        rotationEngineSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void Rotate(float rotationThisFrame)
    {
        myRigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        myRigidbody.freezeRotation = false;
    }
}
