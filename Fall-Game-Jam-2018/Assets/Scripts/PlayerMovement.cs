using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

   
    public float speed;
    public float dashSpeed;
    public float DashCooldownLength;
    public float DashDurationLength = 1.0f;

    private Rigidbody rb;
    private Vector3 playerOldPosition, playerOldVelocity, playerOldAngularVelocity;
    private bool isPlayerOldPositionSet = false;
    
    //counters, updated every tick
    private float DashCooldown = 0.0f;
    private float DashDuration = 0;

    //TimeBased events
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerOldPosition = rb.position;
        playerOldVelocity = rb.velocity;
        playerOldAngularVelocity = rb.angularVelocity;
    }

    private void Update()
    {
        UpdateDash();
    }

    private void FixedUpdate()
    {
        //Input handling
        float moveHorizontal = Input.GetAxis("Horizontal " + gameObject.name);
        float moveVertical = Input.GetAxis("Vertical "+gameObject.name );
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

        if (Input.GetButtonDown("Fire1 "+ gameObject.name))
        {
            TimeTravelAbility();
        }
        if (Input.GetButtonDown("Fire2 "+ gameObject.name))
        {
            DashAbility();
        }
    }

    //Abilities
    private void DashAbility()
    {
        if (DashCooldown == 0.0f)
        {
            float moveHorizontal = Input.GetAxis("Horizontal " + gameObject.name);
            float moveVertical = Input.GetAxis("Vertical " + gameObject.name);
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
 
            rb.velocity += movement * dashSpeed;
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
                playerOldAngularVelocity = rb.angularVelocity;
                isPlayerOldPositionSet = true;

            }
            else
            {
                rb.MovePosition(playerOldPosition);
                rb.velocity = playerOldVelocity;
                rb.angularVelocity = playerOldAngularVelocity;
                isPlayerOldPositionSet = false;
            }
        }
    }

    // Updates to the abilities
    private void UpdateDash()
    {
        // If currently dashing
        if (DashDuration > 0)
        {
            DashDuration -= Time.deltaTime;

            // If end of dashing
            if (DashDuration <= 0)
            {
                Vector3 dash = (dashSpeed / rb.velocity.magnitude) * rb.velocity;
                if (dash.magnitude < rb.velocity.magnitude)
                    rb.velocity -= dash;
                else
                    rb.velocity = Vector3.zero;
                DashCooldown = DashCooldownLength;
                DashDuration = 0;
            }
        }

        // If ability on cooldown
        if (DashCooldown > 0)
        {
            DashCooldown -= Time.deltaTime;

            //If Cooldown is over
            if (DashCooldown < 0)
            {
                DashCooldown = 0;
            }
        }
    }
}
