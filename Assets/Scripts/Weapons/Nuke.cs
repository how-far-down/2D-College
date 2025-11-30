using System.Collections;
using UnityEngine;

public class Nuke : MonoBehaviour
{
    public static Nuke Instance;

    [SerializeField] private CanvasGroup flashCanvas;
    [SerializeField] private float flashSpeed;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private int damageAmount;
    [SerializeField] private float cooldown;
    private float nukeTimer;

    void Awake()
    {
        Instance = this;
        nukeTimer = cooldown;
    }

    public void ActivateNuke()
    {
        StartCoroutine(NukeRoutine());
    }

    private IEnumerator NukeRoutine()
    {
        //FLASH IN
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / flashSpeed;

            flashCanvas.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        
        //DEAL DAMAGE
        DealDamageToAllEnemies();

        //FADE OUT
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeSpeed;
            flashCanvas.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
    
        }
    }

    private void DealDamageToAllEnemies()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
    
        foreach (var enemy in enemies)
        {
            enemy.TakeDamage(damageAmount);
        }
    }

    void Update()
    {
        if (!GameManager.Instance.gameActive) return;

        nukeTimer -= Time.deltaTime;

        if (nukeTimer <= 0f)
        {
            ActivateNuke();
            nukeTimer = cooldown;
        }
    }

}
