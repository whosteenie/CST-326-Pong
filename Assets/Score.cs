using UnityEngine;
using UnityEngine.UIElements;

public class Score : MonoBehaviour {
    private static int _scoreA;
    private static int _scoreB;

    [SerializeField] private GameObject playerA;
    [SerializeField] private GameObject playerB;

    private const float ServePositionA = -2f;
    private const float ServePositionB = 2f;

    [SerializeField] private UIDocument uiDocument;
    private Label _scoreLabel;

    public static bool GameStarted { get; set; }
    public static bool PlayerAServing { get; private set; } = true;

    private void Awake() {
        _scoreLabel = uiDocument.rootVisualElement.Q<Label>("scoreLabel");
    }

    private void OnTriggerEnter(Collider other) {
        var player = "";

        if(other.CompareTag("Ball")) {
            other.gameObject.SetActive(false);

            if(gameObject.CompareTag("WallA")) {
                _scoreB++;
                player = "B";
                PlayerAServing = true;
            } else if(gameObject.CompareTag("WallB")) {
                _scoreA++;
                player = "A";
                PlayerAServing = false;
            }
        }

        if(_scoreA < 11 && _scoreB < 11) {
            Debug.Log($"Player {player} scored! Score: {_scoreB} : {_scoreA}");
        } else {
            Debug.Log($"Game Over! Player {player} wins.");
            _scoreA = 0;
            _scoreB = 0;
        }

        UpdateScoreUI();
        RespawnBall(other.gameObject, player);
    }

    private void UpdateScoreUI() {
        _scoreLabel.text = $"{_scoreB} : {_scoreA}";
    }

    private void RespawnBall(GameObject ball, string playerWhoScored) {
        // The player who was scored on serves next (opposite of who scored)
        var playerScoredOn = playerWhoScored == "A" ? "B" : "A";
        
        ball.transform.parent = playerScoredOn == "A" ? playerA.transform : playerB.transform;

        ball.transform.localPosition = playerScoredOn switch {
            "A" => new Vector3(ServePositionA, 0f, 0f),
            "B" => new Vector3(ServePositionB, 0f, 0f),
            _ => ball.transform.localPosition
        };

        var ballRb = ball.GetComponent<Rigidbody>();
        ballRb.isKinematic = true;
        ballRb.linearVelocity = Vector3.zero;
        ball.SetActive(true);
        GameStarted = false;
    }
}