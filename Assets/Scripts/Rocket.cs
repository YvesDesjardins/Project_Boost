using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float velocity = 100f;
    [SerializeField] float rotation = 50f;
    [SerializeField] AudioClip mainEngine, death, win;
    [SerializeField] ParticleSystem mainEngineParticles, deathParticles, winParticles;
    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
    }

    private void OnCollisionEnter(Collision collision) {
        if (state == State.Alive) {
            switch (collision.gameObject.tag) {
                case "Friendly":
                    break;
                case "Finish":
                    print("hit finish");
                    state = State.Transcending;
                    Invoke("LoadNextScene", 1f); // delays load of the method after 1 second
                    audioSource.Stop();
                    audioSource.PlayOneShot(win);
                    mainEngineParticles.Stop();
                    winParticles.Play();
                    break;
                default:
                    print("boom");
                    state = State.Dying;
                    Invoke("LoadNextScene", 1f);
                    audioSource.Stop();
                    audioSource.PlayOneShot(death);
                    mainEngineParticles.Stop();
                    deathParticles.Play();
                    break;
            }
        }
    }

    private void LoadNextScene() {
        int nextScene;
        if (state == State.Transcending) {
            nextScene = 1;
        }
        else {
            nextScene = 0;
        }
        SceneManager.LoadScene(nextScene);
    }

    private void ProcessInput() {
        if (state == State.Alive) {
            Thrust();
            Rotate();
        }
    }

    private void Thrust() {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) {
            float thrustThisFrame = velocity * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            // prevent audio from layering
            if (!audioSource.isPlaying) {
                audioSource.PlayOneShot(mainEngine);
            }
            mainEngineParticles.Play();
        }
        else {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }
    private void Rotate() {
        float rotationThisFrame;

        if (Input.GetKey(KeyCode.A)) {
            rigidBody.freezeRotation = true;
            rotationThisFrame = rotation * Time.deltaTime;
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) {
            rigidBody.freezeRotation = true;
            rotationThisFrame = rotation * Time.deltaTime;
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }
}
