using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Declaring variables.
    public float speed;
    public float JumpHeight;
    public int lives = 3;
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
    private AudioSource _audio;
    private bool isPlaying = false;
    

    // Start is called before the first frame update
    void Start()
    {
        //Calling components.
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Moving left and right
        float move = Input.GetAxis("Horizontal");
        Vector2 position = transform.position;

        if(move != 0 && !isPlaying && !jumping)
        {
            _audio.Play();
            isPlaying = true;
        }
        else if ((move == 0 && isPlaying) || jumping)
        {
            _audio.Pause();
            isPlaying = false;
        }

        if (position.y < -8.5)
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

    //Taking a powerup item.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isPowerUp && collision.gameObject.tag == "SpeedPowerUp") //Declaring the SpeedPowerUp as the object that gives Player the power up.
        {
            Destroy(collision.gameObject); //Destroying the object, not letting player collect it again.
            speed = speed * 2; //Implementing power up
            isPowerUp = true; //Stating that the powerup is on so that you cant double up on them.
            animator.speed *= 2; //Speeding up the animations.
        }

        if(collision.gameObject.tag == "Checkpoint")
        {
            startPosition = transform.position;
        }

        //Taking away lives as the Player gets hit by the projectiles.
        if(collision.gameObject.name.Contains("EnemyProjectile")) //Making the EnemyProjectile object the object that damages the player.
        {
            lives--; //Taking life awyay
            Debug.Log(lives); //Displays action in console!!!
            ui.UpdateLives(lives); //Calling in the UpdateLives method from UIManager.
            transform.position = startPosition; //Takes the player back to the beginning.
        }

    }

    private void updateAnimator(float move) //Adding values for direction and movement to help change the animations.
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

    //Increasing score with every Pumpkin collected.
    public void CollectPumpkin()
    {
        score++;
        ui.SetScore(score);
    }

}