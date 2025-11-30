using UnityEngine;

public class TrashPrefab : MonoBehaviour
{
    public float speed;
    public float damage;


    public Sprite[] possibleSprites;
    private Transform target;

    void Start()
    {
        //PICK RANDOM SPRITE
        GetComponent<SpriteRenderer>().sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        //MOVE TO TARGET
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
