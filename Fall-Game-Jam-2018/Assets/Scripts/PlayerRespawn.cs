using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerRespawn : MonoBehaviour {

    public Transform spawnPoint;
    public int Deaths = 0;
    public TextMeshProUGUI winText;

    public Image life1;
    public Image life2;
    public Image life3;
    public Image life4;
    public Image life5;

    private static int DEATHS_TO_WIN = 5;
    private GameObject Opponent;

    private Rigidbody rb;
    private bool playerDied;
    private bool gameOver;
    private float timeSinceGameOver;
    private AudioSource[] audioSource;
    private bool hasCalledWonBefore = false;

	// Use this for initialization
	void Start () {
        Opponent = GetComponent<PlayerMovement>().Opponent;
        rb = GetComponent<Rigidbody>();
        gameOver = false;
        timeSinceGameOver = 0.0f;
        audioSource = GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (rb.position.y <= -5)
        {
            playerDied = true;
        }
        if (Deaths == DEATHS_TO_WIN && !hasCalledWonBefore)
        {
            Opponent.GetComponent<PlayerRespawn>().Win();
            hasCalledWonBefore = true;
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
            audioSource[0].Play();
            gameObject.GetComponent<PlayerMovement>().emptyPowerUp();
            rb.MovePosition(spawnPoint.position);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            playerDied = false;
            Deaths++;

            Image lifeToBeLost = null;
            switch (Deaths)
            {
                case 1:
                    lifeToBeLost = life5;
                    break;
                case 2:
                    lifeToBeLost = life4;
                    break;
                case 3:
                    lifeToBeLost = life3;
                    break;
                case 4:
                    lifeToBeLost = life2;
                    break;
                case 5:
                    lifeToBeLost = life1;
                    break;
            }

            if (lifeToBeLost != null)
            {
                lifeToBeLost.gameObject.SetActive(false);
            }
        }
    }

    public void Win()
    {
        string player;
        if (gameObject.name.Contains("1"))
            player = "Player 2";
        else
            player = "Player 1";
        audioSource[6].Play();
        winText.SetText(player + " won the battle");
        gameOver = true;
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
