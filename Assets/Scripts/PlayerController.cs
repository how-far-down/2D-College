using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Base Values")]
    [SerializeField] private float moveSpeed;
    public Vector3 playerMoveDirection;
    public float playerMaxHealth;
    public float playerHealth;

    [Header("Experience")]
    public int experience;
    public int currentLevel;
    public int maxLevel;
    public List<int> playerLevels;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    private float dashCooldownTimer = 0f;
    private bool isDashing = false;

    [Header("Other")]
    public Weapon activeWeapon;
    public bool isDead = false;
    private bool isImmune;
    [SerializeField] private float immunityDuration;
    [SerializeField] private  float immunityTimer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } 
        else 
        {
            Instance = this;
        }
    }
    void Start()
    {
        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count - 1] * 1.1f + 15));
        }
        playerHealth = playerMaxHealth;
        UIController.Instance.UpdateHealthSlider();
        UIController.Instance.UpdateExperienceSlider();
    }

    // Update is called once per frame
    void Update()
    {
        //MOVEMENT
        float inputX  = Input.GetAxisRaw("Horizontal");
        float inputY  = Input.GetAxisRaw("Vertical");
        playerMoveDirection = new Vector3(inputX, inputY).normalized;

        animator.SetFloat("moveX", inputX);
        animator.SetFloat("moveY", inputY);

        if (playerMoveDirection == Vector3.zero)
        {
            animator.SetBool("moving", false);
        } 
        else 
        {
        animator.SetBool("moving", true);    
        }

        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        }
        else
        {
            isImmune = false;
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        //DASHNG INPUT
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift))
        && dashCooldownTimer <= 0f && !isDashing && playerHealth > 0)
        {
            StartCoroutine(DashRoutine());
        }


    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector3(playerMoveDirection.x * 
            moveSpeed, playerMoveDirection.y * moveSpeed);
        }
    }

    //Alows player to take damager
    public void TakeDamage(float damage)
    {
        if (!isImmune)
        {
            isImmune = true;
            immunityTimer = immunityDuration;
            playerHealth -= damage;
            UIController.Instance.UpdateHealthSlider();

            //Ends game if health < 0
            if (playerHealth <= 0)
            {
                StartCoroutine(DeathRoutine());
            }
        }
    }

    //Disables movement, plays death animation, then calls GameOver
    private IEnumerator DeathRoutine()
    {
        moveSpeed = 0f;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        GameManager.Instance.GameOver();
        gameObject.SetActive(false);
  
    }
    

    public void GetExperience(int experienceToGet)
    {
        experience += experienceToGet;
        UIController.Instance.UpdateExperienceSlider();
        if (experience >= playerLevels[currentLevel - 1] && GameManager.Instance.gameActive == true)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        experience -= playerLevels[currentLevel - 1];
        currentLevel++;
        UIController.Instance.UpdateExperienceSlider();
        UIController.Instance.levelUpButtons[0].ActivateButton(activeWeapon);
        UIController.Instance.LevelUpPanelOpen();
    }

    //DASHING COROUTINE
    private IEnumerator DashRoutine()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown;

        Vector3 dashDir = playerMoveDirection;
        if (dashDir == Vector3.zero)
        {
            dashDir = Vector3.up;
        }

        isImmune = true;

        float t = 0f;
        while (t < dashDuration)
        {
            t += Time.deltaTime;
            rb.linearVelocity = dashDir * dashSpeed;
            DamageEnemiesInDash();

            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isImmune = false;
        isDashing = false;
        
    }

    private void DamageEnemiesInDash()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy. transform.position);

            if (distance < 1.0f)
            {
                enemy.TakeDamage(10);
            }
        }
    }
}
