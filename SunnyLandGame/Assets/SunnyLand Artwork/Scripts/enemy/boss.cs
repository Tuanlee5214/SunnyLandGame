
using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Playercontroller player;
    private Rigidbody2D rb;
    private Animator anim;

    private float _timeAttack = 3f;
    private float tocdo = 5f;
    private Vector2 direction;

    private bool isAttacking = false;
    private bool isReturning = false;
    private bool isHurt = false; // Ngăn chặn bị Hurt liên tục
    private float originalY;
    private bool isDie = false;
    private float _timeFirst = 0f;
    private float _timeFire = 3f;

    public GameObject bullet;
    public Transform bossFirePos;
    private AudioManager audioManager;
    public GameObject fx;
    public GameData gameData;
    private Coroutine resetFlyCoroutine;

    // 🔹 Thêm biến Health Bar
    public HealthBar healthBar;
    private float maxHealth = 100f;
    private float currentHealth;

    // 🔹 Biến kiểm tra boss bị trôi lơ lửng quá lâu
    private float floatTime = 0f;
    private float maxFloatTime = 2f; // Thời gian tối đa boss có thể trôi lơ lửng khi tấn công

    void Start()
    {
        player = FindObjectOfType<Playercontroller>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();
        originalY = transform.position.y;

        currentHealth = maxHealth; // Đặt máu đầy đủ lúc đầu
        healthBar.SetMaxHealth(maxHealth); // Cập nhật thanh máu ban đầu

        StartAttackCycle(); // Bắt đầu vòng lặp tấn công
    }

    void Update()
    {
        if (!isAttacking && !isReturning)
        {
            MaintainFlyingState();
        }
        Flip();

        if (currentHealth <= 0)
        {
            Die();
        }

        if (!isAttacking && !isReturning && !isHurt && player.GetIsGrounded())
        {
            StartAttackCycle();
        }

        CheckFloatingAttack(); // Kiểm tra nếu boss bị trôi quá lâu
    }

    private void MaintainFlyingState()
    {
        if (transform.position.y >= originalY)
        {
            SetAnimationState(true, false, false);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            _timeFirst += 1/60;
            if (_timeFirst >= _timeFire)
            {
                Fire();
                _timeFirst = 0;
            }
        }
    }

    private void MinusHealth(int points)
    {
        currentHealth -= points;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo không âm
        healthBar.SetHealth(currentHealth); // Cập nhật thanh máu
    }

    private void Fire()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        if (bullet == null)
        {
            Debug.Log("looix");
            return;
        }
        if (gameData.health == 0) return;
        audioManager.PlayBossAttack();
        GameObject bullet1 = Instantiate(bullet, bossFirePos.transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet1.GetComponent<Rigidbody2D>();
        rb.velocity = tocdo * 2.4f * direction;
    }

    private void Die()
    {
        GameObject fx1 = Instantiate(fx, transform.position, Quaternion.identity);
        Destroy(gameObject);
        audioManager.PlayKillEnemySound();
        Destroy(fx1, 0.5f);
        isDie = true;
    }

    public bool GetIsDie()
    {
        return isDie;
    }

    private void StartAttackCycle()
    {
        if (!isAttacking && !isReturning && !isHurt)
        {
            Invoke(nameof(Attack), _timeAttack);
        }
    }

    public void Attack()
    {
        if (!isAttacking && !isReturning && !isHurt && player.GetIsGrounded())
        {
            isAttacking = true;
            floatTime = 0f; // Reset bộ đếm khi bắt đầu tấn công
            direction = (player.transform.position - transform.position).normalized;
            SetAnimationState(false, true, false);

            rb.gravityScale = 0;
            rb.velocity = direction * tocdo * 3.5f;
        }
    }

    private void CheckFloatingAttack()
    {
        if (isAttacking)
        {
            floatTime += Time.deltaTime;

            if (floatTime >= maxFloatTime) // Nếu vượt quá thời gian trôi lơ lửng tối đa
            {
                isAttacking = false;
                isReturning = false;
                SetAnimationState(true, false, false);
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                transform.position = new Vector2(transform.position.x, originalY); // Đưa boss về độ cao ban đầu

                StartAttackCycle(); // Tiếp tục vòng lặp tấn công
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAttacking && (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Player") && !player.GetIsFalling())))
        {
            isAttacking = false;
            isReturning = true;

            SetAnimationState(true, false, false);
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 3f);

            StartCoroutine(ResetToIdleState());
        }

        if (collision.gameObject.CompareTag("Player") && player.GetIsFalling() && !isHurt)
        {
            isHurt = true;
            isAttacking = false;
            isReturning = true;
            SetAnimationState(false, false, true);
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            audioManager.PlayBossHurtSound();
            MinusHealth(20); // 🔹 Trừ 20 máu mỗi lần Boss bị thương

            StartResetFlyCoroutine();
        }
    }

    private void StartResetFlyCoroutine()
    {
        if (resetFlyCoroutine != null)
        {
            StopCoroutine(resetFlyCoroutine);
        }
        resetFlyCoroutine = StartCoroutine(ResetFlyState());
    }

    private IEnumerator ResetToIdleState()
    {
        yield return new WaitForSeconds(1f);
        rb.gravityScale = 0;
        isReturning = false;
        StartAttackCycle(); // Tiếp tục tấn công sau khi reset
    }

    private IEnumerator ResetFlyState()
    {
        yield return new WaitForSeconds(1f);

        while (transform.position.y < originalY - 0.1f)
        {
            rb.velocity = new Vector2(0, 2.85f);
            yield return null;
        }

        rb.velocity = Vector2.zero;
        transform.position = new Vector2(transform.position.x, originalY);

        isReturning = false;
        isHurt = false;

        SetAnimationState(true, false, false);

        StartAttackCycle(); // Tiếp tục tấn công sau khi bị hurt
    }

    private void SetAnimationState(bool isFly, bool isAttack, bool isHurt)
    {
        anim.SetBool("IsFly", isFly);
        anim.SetBool("IsAttack", isAttack);
        anim.SetBool("IsHurt", isHurt);
    }

    void Flip()
    {
        if ((player.transform.position.x < transform.position.x && transform.localScale.x < 0) ||
            (player.transform.position.x > transform.position.x && transform.localScale.x > 0))
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
    }
}





