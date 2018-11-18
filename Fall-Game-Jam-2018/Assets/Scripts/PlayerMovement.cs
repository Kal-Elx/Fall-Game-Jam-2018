using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public Slider powerupSlider;
    public float speed;

    //Dash public variables
    public float dashSpeed;
    public float DashCooldownLength = 2;
    public float DashDurationLength = 1.0f;

    // Stop public variables
    public GameObject Opponent;
    public float StopCooldownLength = 2;
    public float StopDurationLength = 1.0f;

    //TimeAbility public variables
    public float TimeCooldownLength = 2;

    private Rigidbody rb;
    private Vector3 playerOldPosition, playerOldVelocity, playerOldAngularVelocity;
    private Vector3 playerStopVelocity, playerStopAngularVelocity;
    private bool isPlayerOldPositionSet = false;
    
    //counters, updated every tick
    private float DashCooldown = 0.0f;
    private float DashDuration = 0.0f;
    private float StopCooldown = 0.0f;
    private float StopDuration = 0.0f;
    private float TimeCoolDown = 0.0f;

    private int powerupCounter;


    private bool Stopped = false;


    

    //TimeBased events
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerOldPosition = rb.position;
        playerOldVelocity = rb.velocity;
        playerOldAngularVelocity = rb.angularVelocity;
        powerupCounter = 0;
    }

    private void Update()
    {

        UpdateStopAbility();
        UpdateTimeAbility();
        UpdateDash();

        if (Input.GetKey(KeyCode.Escape)) {
            SceneManager.LoadScene("MenuScene");
        }
    }

    private void FixedUpdate()
    {

        if (!Stopped)
        {
            //Input handling
            float moveHorizontal = Input.GetAxis("Horizontal " + gameObject.name);
            float moveVertical = Input.GetAxis("Vertical " + gameObject.name);
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * speed);

            if (Input.GetButtonDown("Fire1 " + gameObject.name))
            {
                TimeTravelAbility();
            }
            if (Input.GetButtonDown("Fire2 " + gameObject.name))
            {
                DashAbility();
            }

            if (Input.GetButtonDown("Fire3 " + gameObject.name))
            {
                StopAbility();
            }
        }

    }

    //Abilities
    private void DashAbility()
    {
        if (DashCooldown <= 0.0f && ableToUsePowerUp())
        {
            float moveHorizontal = Input.GetAxis("Horizontal " + gameObject.name);
            float moveVertical = Input.GetAxis("Vertical " + gameObject.name);
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            if (!movement.Equals(Vector3.zero))
            {
                rb.velocity += movement * dashSpeed;
                DashCooldown = DashCooldownLength;
                DashDuration = DashDurationLength;
            }
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
            else if (TimeCoolDown <= 0.0f && ableToUsePowerUp())
            {
                TimeCoolDown = TimeCooldownLength;
                rb.MovePosition(playerOldPosition);
                rb.velocity = playerOldVelocity;
                rb.angularVelocity = playerOldAngularVelocity;
                isPlayerOldPositionSet = false;
            }
        }
    }

    private void StopAbility()
    {
        if (StopCooldown<=0.0f && ableToUsePowerUp())
        {
            StopCooldown = StopCooldownLength;
            Opponent.GetComponent<PlayerMovement>().stop();
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
    private void UpdateStopAbility()
    {
        if (StopDuration > 0)
        {
            StopDuration -= Time.deltaTime;

            // If end of stop
            if (StopDuration <= 0)
                unStop();

            if (StopCooldown > 0)
            {
                StopCooldown -= Time.deltaTime;

                //If Cooldown is over
                if (StopCooldown < 0)
                {
                    StopCooldown = 0;
                }
            }
        }
    }

    private void UpdateTimeAbility()
    {
        if (TimeCoolDown > 0)
        {
            TimeCoolDown -= Time.deltaTime;

            //If Cooldown is over
            if (TimeCoolDown < 0)
            {
                TimeCoolDown = 0;
            }
        }
    }

    public void stop()
    {
        Stopped = true;
        rb.isKinematic=true;
        StopDuration = StopDurationLength;
    }

    private void unStop() {

        Stopped = false;
        StopDuration = 0;
        rb.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            other.gameObject.SetActive(false);
            powerUpCollected();
        }
    }

    private void powerUpCollected()
    {
        if (powerupCounter <= 10)
        {
            ++powerupCounter;
            ++powerupSlider.value;
        }
    }

    private bool ableToUsePowerUp()
    {
        if (powerupCounter > 0)
        {
            --powerupCounter;
            --powerupSlider.value;
            return true;
        }
        else
        {
            return false;
        }    
    }

    public void emptyPowerUp()
    {
        while (ableToUsePowerUp())
        {
            //a hacky way to empty the power up meter, maybe fix later
        }
    }

}