using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour {
    [SerializeField] private GameObject ball;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxLaunchAngle = 45f;

    private const float MoveSpeed = 25f;

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

        rb.linearVelocity = move * MoveSpeed;
    }

    private void OnJump(InputValue value) {
        if(Score.GameStarted) return;
        
        // Only the serving paddle can launch the ball
        if(Score.PlayerAServing && transform.position.x > 0) return;
        if(!Score.PlayerAServing && transform.position.x < 0) return;

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