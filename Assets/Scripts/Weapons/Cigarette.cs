using UnityEngine;

public class Cigarette : Weapon
{
    [SerializeField] private GameObject prefab;
    private float spawnCounter;

    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = stats[weaponLevel].cooldown;
            Instantiate(prefab, transform.position, transform.rotation, transform);
        }
    }
}









