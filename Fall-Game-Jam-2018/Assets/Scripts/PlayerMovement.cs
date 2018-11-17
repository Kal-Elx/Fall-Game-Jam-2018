using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    
    public float speed;

    private Rigidbody rb;
    private Vector3 playerOldPosition, playerOldVelocity;
    public bool isPlayerOldPositionSet = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerOldPosition = rb.position;
        playerOldVelocity = rb.velocity;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);


        if (Input.GetButtonDown("Fire1"))
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
