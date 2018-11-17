using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour {

    public Transform spawnPoint;

    private Rigidbody rb;
    private bool playerDied;
    

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (rb.position.y <= -5)
        {
            playerDied = true;
        }

    }

    private void FixedUpdate()
    {
        if (playerDied)
        {
            rb.MovePosition(spawnPoint.position);
            rb.velocity=new Vector3(0, 0, 0);
            playerDied = false;
        }

    }
}
