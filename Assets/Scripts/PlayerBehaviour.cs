using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public UnityEvent OnJump;

    [Header("Controls")]
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;

    [Header("Platform Detection")]
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    public Transform spawnPoint;
    public Transform lookAheadPoint;
    public Transform lookInFrontPoint;
    public LayerMask collisionGroundLayer;
    public LayerMask collisionWallLayer;
    public RampDirection rampDirection;
    public bool onRamp;

    //[Header("Player Stats")]
    //public int lives;
    //public int health;
    //public BarController healthBar;
    //public Animator livesHUD;

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    private RaycastHit2D groundHit;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _LookInFront();
        _LookDown();
        _Move();
    }

    private void _LookInFront()
    {
        if (!isGrounded)
        {
            rampDirection = RampDirection.NONE;
            return;
        }

        var wallHit = Physics2D.Linecast(transform.position, lookInFrontPoint.position, collisionWallLayer);
        if (wallHit && isOnSlope())
        {
            rampDirection = RampDirection.UP;
        }
        else if (!wallHit && isOnSlope())
        {
            rampDirection = RampDirection.DOWN;
        }

        Debug.DrawLine(transform.position, lookInFrontPoint.position, Color.red);
    }

    private void _LookDown()
    {
        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.green);
        groundHit = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionGroundLayer);

        isGrounded = (groundHit) ? true : false;

        if (m_animator.GetBool("isFalling") && isGrounded)
        {
            m_animator.SetBool("isLanding", true);
            m_animator.SetBool("isFalling", false);
        }
    }

    private bool isOnSlope()
    {
        if (!isGrounded)
        {
            onRamp = false;
            return false;
        }

        if (groundHit.normal != Vector2.up)
        {
            onRamp = true;
            return true;
        }

        onRamp = false;
        return false;
    }

    void _Move()
    {
        if (isGrounded)
        {
            if (!isJumping && !isCrouching)
            {
                if (joystick.Horizontal > joystickHorizontalSensitivity)
                {
                    // move right
                    m_rigidBody2D.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    if (onRamp && rampDirection == RampDirection.UP)
                    {
                        m_rigidBody2D.AddForce(Vector2.up * horizontalForce * 0.5f * Time.deltaTime);
                    }
                    else if (onRamp && rampDirection == RampDirection.DOWN)
                    {
                        m_rigidBody2D.AddForce(Vector2.down * horizontalForce * 0.5f * Time.deltaTime);
                    }


                    m_animator.SetInteger("MoveState", (int)PlayerAnimationType.RUN);
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    // move left
                    m_rigidBody2D.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    if (onRamp && rampDirection == RampDirection.UP)
                    {
                        m_rigidBody2D.AddForce(Vector2.up * horizontalForce * 1.0f * Time.deltaTime);
                    }
                    else if (onRamp && rampDirection == RampDirection.DOWN)
                    {
                        m_rigidBody2D.AddForce(Vector2.down * horizontalForce * 0.5f * Time.deltaTime);
                    }

                    m_animator.SetInteger("MoveState", (int)PlayerAnimationType.RUN);
                }
                else
                {
                    m_animator.SetInteger("MoveState", (int)PlayerAnimationType.IDLE);
                }
            }

            if ((joystick.Vertical > joystickVerticalSensitivity) && (!isJumping))
            {
                // jump
                m_rigidBody2D.AddForce(Vector2.up * verticalForce);
                m_animator.SetTrigger("isJumping");
                isJumping = true;
                OnJump.Invoke();
            }
            else
            {
                isJumping = false;
            }

            if ((joystick.Vertical < -joystickVerticalSensitivity) && (!isCrouching))
            {
                m_animator.SetInteger("MoveState", (int)PlayerAnimationType.CROUCH);
                isCrouching = true;
            }
            else
            {
                isCrouching = false;
            }
        }
    }

    //public void LoseLife()
    //{
    //    lives -= 1;

    //    livesHUD.SetInteger("LivesState", lives);

    //    if (lives > 0)
    //    {
    //        health = 100;
    //        healthBar.SetValue(health);

    //        transform.position = spawnPoint.position;
    //    }
    //    else
    //    {
    //        SceneManager.LoadScene("End");
    //    }
    //}

    //public void TakeDamage(int damage)
    //{
    //    health -= damage;
    //    healthBar.SetValue(health);

    //    if (health <= 0)
    //        LoseLife();
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //TakeDamage(15);
        }

        if (isGrounded && groundHit.collider.gameObject.layer == 15) // Platform_Bounce layer
        {
            m_rigidBody2D.AddForce(Vector2.up * verticalForce * 3.0f);
            if(transform.localScale.x < 0)
                m_rigidBody2D.AddForce(Vector2.left * horizontalForce * 0.2f);
            else
                m_rigidBody2D.AddForce(Vector2.right * horizontalForce * 0.2f);
            m_animator.SetTrigger("isJumping");
            isJumping = true;

            collision.gameObject.GetComponent<Animator>().SetBool("isBouncing", true);
        }
        else if (isGrounded && groundHit.collider.gameObject.layer == 16) // Platform_Bomb layer
        {
            collision.gameObject.GetComponent<BombPlatformBehaviour>().Countdown();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // respawn
        if (other.gameObject.CompareTag("Death Floor"))
        {
            transform.position = spawnPoint.position;
            //LoseLife();
        }

        if(other.gameObject.CompareTag("Bullet"))
        {
            //TakeDamage(10);
        }

        if(other.gameObject.CompareTag("Platform"))
        {
            //TakeDamage(20);
        }
    }
}
