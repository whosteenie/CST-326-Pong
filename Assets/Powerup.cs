using UnityEngine;

public enum PowerupType { SpeedBoost, DoublePoints }

public class Powerup : MonoBehaviour {
    public PowerupType type;

    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Player")) return;
        
        var paddle = other.GetComponent<Paddle>();
        if(paddle == null) return;
        
        paddle.ActivatePowerup(type);
        Destroy(gameObject);
    }
}
