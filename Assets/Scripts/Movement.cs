using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] AudioClip mainEngine;

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
            myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }

            if (!mainEngineParticles.isPlaying)
            {
                mainEngineParticles.Play();
            }
     
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    //Processes input to make spacecraft rotate
    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(rotateSpeed);
            if (!rightEngineParticles.isPlaying)
            {
                rightEngineParticles.Play();
            }
            
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Rotate(-rotateSpeed);
            if (!leftEngineParticles.isPlaying)
            {
                leftEngineParticles.Play();
            }
            
        }
        else
        {
            leftEngineParticles.Stop();
            rightEngineParticles.Stop();
        }
        
    }

    void Rotate(float rotationThisFrame)
    {
        myRigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        myRigidbody.freezeRotation = false;
    }
}
