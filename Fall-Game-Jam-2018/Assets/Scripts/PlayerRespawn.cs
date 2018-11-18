using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour {

    public Transform spawnPoint;
    public int Deaths = 0;
    public TextMeshProUGUI winText;

    private static int DEATHS_TO_WIN = 5;
    private GameObject Opponent;

    private Rigidbody rb;
    private bool playerDied;
    private bool gameOver;
    private float timeSinceGameOver;

	// Use this for initialization
	void Start () {
        Opponent = GetComponent<PlayerMovement>().Opponent;
        rb = GetComponent<Rigidbody>();
        gameOver = false;
        timeSinceGameOver = 0.0f;
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
        if (gameOver)
        {
            timeSinceGameOver += Time.deltaTime;
            if (timeSinceGameOver > 5)
            {
                BackToMainMenu();
            }
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
        string player;
        if (gameObject.name.Contains("1"))
            player = "Player 1";
        else
            player = "Player 2";
        winText.SetText(player + " won the battle");
        gameOver = true;
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
