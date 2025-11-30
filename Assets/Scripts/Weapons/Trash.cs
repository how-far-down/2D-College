using UnityEngine;
using System.Linq;

public class Trash : MonoBehaviour
{
    public GameObject TrashPrefab;

    [Header("Stats")]

    public float projectileSpeed = 10f;
    public float projectileDamage = 10f;
    public float shootCooldown = 0.5f;
    public float range = 10f;



    float cooldownTimer;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            ShootNearestEnemy();
            cooldownTimer = shootCooldown;
        }
    }

    void ShootNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        Transform nearest = enemies
            .OrderBy(e => Vector2.Distance(transform.position, e.transform.position))
            .First()
            .transform;

        GameObject p = Instantiate(TrashPrefab, transform.position, Quaternion.identity);
        p.GetComponent<TrashPrefab>().SetTarget(nearest);
    }
}
