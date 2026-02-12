using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour {
    [SerializeField] private GameObject ball;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxLaunchAngle = 45f;
    [SerializeField] private MeshRenderer meshRenderer;

    private const float BaseMoveSpeed = 25f;
    private float _currentMoveSpeed = BaseMoveSpeed;
    private Color _originalColor;

    public bool IsDoublePointsActive { get; private set; }

    private void Start() {
        if (meshRenderer != null) {
            _originalColor = meshRenderer.material.color;
        }
    }

    private void Update() {
        var direction = 0f;

        if(transform.position.x > 0) {
            // Right paddle
            if(Keyboard.current.upArrowKey.isPressed) {
                direction = 1f;
            }

            if(Keyboard.current.downArrowKey.isPressed) {
                direction = -1f;
            }
        } else {
            // Left paddle
            if(Keyboard.current.wKey.isPressed) {
                direction = 1f;
            }

            if(Keyboard.current.sKey.isPressed) {
                direction = -1f;
            }
        }

        var move = new Vector3(0, 0, direction);

        rb.linearVelocity = move * _currentMoveSpeed;
    }

    public void ActivatePowerup(PowerupType type) {
        StopAllCoroutines();
        StartCoroutine(PowerupRoutine(type));
    }

    private System.Collections.IEnumerator PowerupRoutine(PowerupType type) {
        switch(type) {
            case PowerupType.SpeedBoost:
                _currentMoveSpeed = BaseMoveSpeed * 2f;
                meshRenderer.material.color = Color.blue;
                break;
            case PowerupType.DoublePoints:
                IsDoublePointsActive = true;
                meshRenderer.material.color = Color.red;
                break;
        }

        yield return new WaitForSeconds(10f);

        _currentMoveSpeed = BaseMoveSpeed;
        IsDoublePointsActive = false;
        meshRenderer.material.color = _originalColor;
    }

    private void OnJump(InputValue value) {
        if(Score.GameStarted) return;

        switch(Score.PlayerAServing) {
            // Only the serving paddle can launch the ball
            case true when transform.position.x > 0:
            case false when transform.position.x < 0:
                return;
        }

        // Launch the ball
        Score.GameStarted = true;

        ball.transform.parent = null;

        var rbBall = ball.GetComponent<Rigidbody>();
        rbBall.isKinematic = false;

        var baseDirection = ball.transform.position.x > 0 ? Vector3.left : Vector3.right;
        var randomAngle = Random.Range(-maxLaunchAngle, maxLaunchAngle);
        var launchDirection = Quaternion.Euler(0, randomAngle, 0) * baseDirection;
        ball.GetComponent<Ball>().Serve(launchDirection);
    }
}