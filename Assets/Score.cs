using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class Score : MonoBehaviour {
    private static int _scoreA;
    private static int _scoreB;

    [SerializeField] private GameObject playerA;
    [SerializeField] private GameObject playerB;

    private const float ServePositionA = -2f;
    private const float ServePositionB = 2f;

    [SerializeField] private UIDocument uiDocument;
    private Label _scoreALabel;
    private Label _scoreBLabel;

    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float highlightDuration = 0.5f;

    public static bool GameStarted { get; set; }
    public static bool PlayerAServing { get; private set; } = true;

    private void Awake() {
        var root = uiDocument.rootVisualElement;
        _scoreALabel = root.Q<Label>("scoreALabel");
        _scoreBLabel = root.Q<Label>("scoreBLabel");
    }

    private void OnTriggerEnter(Collider other) {
        var player = "";

        if(other.CompareTag("Ball")) {
            other.gameObject.SetActive(false);

            if(gameObject.CompareTag("WallA")) {
                var paddleB = playerB.GetComponent<Paddle>();
                var increment = (paddleB != null && paddleB.IsDoublePointsActive) ? 2 : 1;
                _scoreB += increment;
                player = "B";
                PlayerAServing = true;
                StartCoroutine(HighlightScore(_scoreBLabel));
            } else if(gameObject.CompareTag("WallB")) {
                var paddleA = playerA.GetComponent<Paddle>();
                var increment = (paddleA != null && paddleA.IsDoublePointsActive) ? 2 : 1;
                _scoreA += increment;
                player = "A";
                PlayerAServing = false;
                StartCoroutine(HighlightScore(_scoreALabel));
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
        if(_scoreALabel != null) _scoreALabel.text = _scoreA.ToString();
        if(_scoreBLabel != null) _scoreBLabel.text = _scoreB.ToString();
    }

    private IEnumerator HighlightScore(Label label) {
        if(label == null) yield break;
        
        label.style.color = highlightColor;
        yield return new WaitForSeconds(highlightDuration);
        label.style.color = Color.white;
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