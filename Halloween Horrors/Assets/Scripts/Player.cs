using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float JumpHeight;
    private Rigidbody2D rb;
    private bool jumping = false;
    private Animator animator;
    private int score = 0;
    [SerializeField] private UIManager ui;
    public GameObject projectilePrefab;
    private bool isPowerUp = false;
    private float powerUpTimeRemaining = 5;
    private float DefaultPowerUpTime = 5;
    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        Vector2 position = transform.position;
        if(position.y < -8.5)
        {
            position = startPosition;
        }
        else
        {
            position.x = position.x + (speed * Time.deltaTime * move);
        }
        transform.position = position;
        updateAnimator(move);

        if(Input.GetKeyDown(KeyCode.Space) && jumping == false)
        {
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2 * Physics2D.gravity.y * JumpHeight)), ForceMode2D.Impulse);
            jumping = true;
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            GameObject projectile = Instantiate(projectilePrefab, rb.position, Quaternion.identity);
            Projectile pr = projectile.GetComponent<Projectile>();
            pr.Launch(new Vector2(animator.GetInteger("Direction"), 0), 300);
        }

        if (isPowerUp)
        {
            powerUpTimeRemaining -= Time.deltaTime;
            if(powerUpTimeRemaining < 0)
            {
                isPowerUp = false;
                powerUpTimeRemaining = DefaultPowerUpTime;
                animator.speed /= 2;
                speed /= 2;
            }
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isPowerUp && collision.gameObject.tag == "SpeedPowerUp")
        {
            Destroy(collision.gameObject);
            speed = speed * 2;
            isPowerUp = true;
            animator.speed *= 2;
        }
    }

    private void updateAnimator(float move)
    {
        animator.SetFloat("Move", move);
        if(move > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (move < 0)
        {
            animator.SetInteger("Direction", -1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumping = false;
    }

    public void CollectPumpkin()
    {
        score++;
        ui.SetScore(score);
    }

}