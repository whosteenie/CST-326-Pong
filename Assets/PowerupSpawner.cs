using UnityEngine;

public class PowerupSpawner : MonoBehaviour {
    [SerializeField] private GameObject speedPowerupPrefab;
    [SerializeField] private GameObject doublePointsPowerupPrefab;
    
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float zRange = 14f;
    [SerializeField] private float xLeft = -28f;
    [SerializeField] private float xRight = 28f;

    private void Start() {
        InvokeRepeating(nameof(SpawnPowerup), spawnInterval, spawnInterval);
    }

    private void SpawnPowerup() {
        var prefab = Random.value > 0.5f ? speedPowerupPrefab : doublePointsPowerupPrefab;
        var x = Random.value > 0.5f ? xLeft : xRight;
        var z = Random.Range(-zRange, zRange);
        
        var spawnPos = new Vector3(x, 0.5f, z);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
