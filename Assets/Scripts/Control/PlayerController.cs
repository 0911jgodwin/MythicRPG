using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator anim;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;

    public float speed = 3f;
    public float backwardsSpeedModifier = 0.5f;
    public float gravity = -9.81f;

    public float horizontal;
    public float vertical;

    Vector3 velocity;
    bool isGrounded;

    void FixedUpdate()
    {
        Rotate();
        JumpHandling();
        Move();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }

    private void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
        }
    }

    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            anim.SetBool("moving", true);
            anim.SetFloat("vertical", vertical);
            anim.SetFloat("horizontal", horizontal);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + transform.eulerAngles.y;

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (direction.z < 0)
            {
                controller.Move(moveDir.normalized * (speed*backwardsSpeedModifier) * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    private void JumpHandling()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        anim.SetFloat("jump", velocity.y);
        anim.SetBool("grounded", isGrounded);
        controller.Move(velocity * Time.deltaTime);
    }
}