using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip successAudio;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;
    bool isControllable = true;
    bool isCollidable = true;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // RespondToDebugKeys();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isControllable || !isCollidable)
        {
            return;
        }

        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Everything is good!");
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successAudio);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        SceneManager.LoadScene(nextScene);
        
    }


    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashAudio);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    // void RespondToDebugKeys()
    // {
    //     if (Keyboard.current.lKey.wasPressedThisFrame)
    //     {
    //         LoadNextLevel();
    //     } else if (Keyboard.current.cKey.wasPressedThisFrame)
    //     {
    //         isCollidable = !isCollidable;
    //     }
    // }   
}
