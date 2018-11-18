using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {


    //Public Variables
    public float speed;

    //Dash variables
    public float dashSpeed;
    public float DashCooldownLength = 2;
    public float DashDurationLength = 1.0f;

    // Stop variables
    public GameObject Opponent;
    public float StopCooldownLength = 2;
    public float StopDurationLength = 1.0f;

    //TimeAbility variables
    public float TimeCooldownLength = 2;

    //Sliders
    public Slider powerupSlider;
    public Slider timetravelSlider;
    public Slider dashSlider;
    public Slider freezeSlider;


    //Private variables
    private Rigidbody rb;
    private AudioSource[] audioSource;

    //Timetravel variables
    private Vector3 playerOldPosition, playerOldVelocity, playerOldAngularVelocity;
    private bool isPlayerOldPositionSet = false;
    
    //counters for abilites, updated every tick
    //Cooldowns
    private float DashCooldown = 0.0f;
    private float TimeCoolDown = 0.0f;
    private float StopCooldown = 0.0f;

    //Durations
    private float DashDuration = 0.0f;
    private float StopDuration = 0.0f;
    
    // counter for the powerup meter;
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
        audioSource = GetComponents<AudioSource>();

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
        if (DashCooldown <= 0.0f)
        {
            float moveHorizontal = Input.GetAxis("Horizontal " + gameObject.name);
            float moveVertical = Input.GetAxis("Vertical " + gameObject.name);
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            if (!movement.Equals(Vector3.zero) && ableToUsePowerUp())
            {
                audioSource[3].Play();
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
                audioSource[5].Play();
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
            audioSource[4].Play();
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
            dashSlider.value = 2 - DashCooldown;
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
        }

        if (StopCooldown > 0)
        {
            StopCooldown -= Time.deltaTime;

            //If Cooldown is over
            if (StopCooldown < 0)
            {
                StopCooldown = 0;
            }
            freezeSlider.value = 2 - StopCooldown;
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
            timetravelSlider.value = 2 - TimeCoolDown;
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
            audioSource[2].Play();
            other.gameObject.SetActive(false);
            powerUpCollected();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            audioSource[1].Play();
        
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