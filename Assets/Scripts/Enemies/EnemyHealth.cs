using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject robotExplosionVFX;
    [SerializeField] GameObject AmmoPickupPrefab;
    [SerializeField] int startingHealth = 10;
    
    int currentHealth;

    GameManager gameManager;

    void Awake()
    {
        currentHealth = startingHealth;
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.AdjustEnemiesLeft(1);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            SelfDestruct();
        }
    }

    public void SelfDestruct()
    {
        Instantiate(robotExplosionVFX, transform.position, Quaternion.identity);
        gameManager.AdjustEnemiesLeft(-1);
        RandomAmmoPickup();
        Destroy(this.gameObject);
    }

    public void RandomAmmoPickup()
    {
        if (AmmoPickupPrefab == null) return;

        // Random tỉ lệ 60%
        float chance = Random.value; // Từ 0.0 đến 1.0
        if (chance < 0.6f) return; // 50% không spawn

        Instantiate(AmmoPickupPrefab, transform.position, Quaternion.identity);
    }
}