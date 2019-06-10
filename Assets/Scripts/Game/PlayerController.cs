using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode kLeft, kRight, kJump, kSprint;

    [Header("Settings")]
    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float fallMultiplier = 2.5f;
    [SerializeField]
    [Range(0, 1)]
    private float breakingSpeed;
    [SerializeField]
    private float groundCheckDistance;


    private float currentSpeed;
    private bool isGrounded;
    private Vector2 newVelocity;


    [Header("Objects")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Rigidbody2D rigidbody2D;

    private void Start()
    {
        currentSpeed = walkingSpeed;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        newVelocity = rigidbody2D.velocity;
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);
        Debug.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance), isGrounded ? Color.green : Color.red);
        //Jumping
        if (isGrounded && Input.GetKeyDown(kJump))
        {
            newVelocity = Vector2.up * jumpHeight;
        }

        if (rigidbody2D.velocity.y < 0)
        {
            newVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidbody2D.velocity.y > 0 && !Input.GetKey(kJump))
        {
            newVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        //Setting the movement speed
        currentSpeed = Input.GetKey(kSprint) ? runningSpeed : walkingSpeed;

        //Slowing down the left and right speed (This does effect the movement speed)
        newVelocity.x = Mathf.Lerp(newVelocity.x, 0, breakingSpeed);

        //Left and Right Movement
        if (Input.GetKey(kLeft))
        {
            newVelocity.x += Vector2.left.x * currentSpeed;
        }
        else if (Input.GetKey(kRight))
        {
            newVelocity.x -= Vector2.left.x * currentSpeed;
        }

        rigidbody2D.velocity = newVelocity;

        
    }
}
