using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour {

    public Transform spawnPoint;
    public int Deaths = 0;
    private static int DEATHS_TO_WIN = 5;
    private GameObject Opponent;

    private Rigidbody rb;
    private bool playerDied;
    

	// Use this for initialization
	void Start () {
        Opponent = GetComponent<PlayerMovement>().Opponent;
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (rb.position.y <= -5)
        {
            playerDied = true;
        }
        if (Deaths == DEATHS_TO_WIN)
        {
            Opponent.GetComponent<PlayerRespawn>().Win();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }

    private void FixedUpdate()
    {
    
    }
    private void LateUpdate()
    {
        if (playerDied)
        {
            gameObject.GetComponent<PlayerMovement>().emptyPowerUp();
            rb.MovePosition(spawnPoint.position);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            playerDied = false;
            Deaths++;
        }
    }

    public void Win()
    {
        Debug.Log(gameObject.name + "Won");

      //TODO: Do stuff when game won   
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
