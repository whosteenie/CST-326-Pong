using UnityEngine;

public class Ball : MonoBehaviour {
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float initialSpeed = 20f;
    [SerializeField] private float speedIncrement = 1f;

    private float _currentSpeed;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float basePitch = 1f;
    [SerializeField] private float pitchMultiplier = 0.05f;

    private void Start() {
        _currentSpeed = initialSpeed;
        if(Score.GameStarted) return;

        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
    }

    private void FixedUpdate() {
        if(!rb.isKinematic && rb.linearVelocity.sqrMagnitude > 0.001f) {
            rb.linearVelocity = rb.linearVelocity.normalized * _currentSpeed;
        }
    }

    private void OnCollisionEnter(Collision other) {
        _currentSpeed += speedIncrement;

        var normal = other.contacts[0].normal;
        rb.linearVelocity = Vector3.Reflect(rb.linearVelocity, normal);

        // Audio feedback only for paddle collisions
        if(!other.gameObject.CompareTag("Player")) return;
        audioSource.pitch = basePitch + (_currentSpeed - initialSpeed) * pitchMultiplier;
        audioSource.Play();
    }

    public void Serve(Vector3 direction) {
        _currentSpeed = initialSpeed;
        rb.linearVelocity = direction.normalized * _currentSpeed;
    }
}