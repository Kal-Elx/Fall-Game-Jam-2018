using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    
    public float speed;
    public float dashSpeed;
    public float DashCooldownLength;
    public float DashDurationLength = 1.0f;

    private Rigidbody rb;
    private Vector3 playerOldPosition, playerOldVelocity;
    private bool isPlayerOldPositionSet = false;
    private float DashCooldown = 0.0f;
    private float DashDuration = 0;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerOldPosition = rb.position;
        playerOldVelocity = rb.velocity;
    }

    private void Update()
    {
        if (DashDuration > 0)
        {
            DashDuration -= Time.deltaTime;
            if (DashDuration <= 0)
            {
                rb.velocity -= (dashSpeed / rb.velocity.magnitude) * rb.velocity;
                DashCooldown = DashCooldownLength;
                DashDuration = 0;
            }
        }
        if (DashCooldown > 0)
        {
            DashCooldown -= Time.deltaTime;

            if(DashCooldown < 0)
            {
                DashCooldown = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);


        if (Input.GetButtonDown("Fire1"))
        {
            TimeTravelAbility();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            DashAbility();
        }
    }


    private void DashAbility()
    {
        if (DashCooldown == 0.0f)
        {
            rb.velocity += (dashSpeed / rb.velocity.magnitude) * rb.velocity;
            DashCooldown = DashCooldownLength;
            DashDuration = DashDurationLength;
        }
    }

    private void TimeTravelAbility()
    {
        if (DashDuration == 0)
        {
            if (!isPlayerOldPositionSet)
            {
                playerOldPosition = rb.position;
                playerOldVelocity = rb.velocity;

                isPlayerOldPositionSet = true;

            }
            else
            {
                rb.MovePosition(playerOldPosition);
                rb.velocity = playerOldVelocity;
                isPlayerOldPositionSet = false;
            }
        }
    }
}
