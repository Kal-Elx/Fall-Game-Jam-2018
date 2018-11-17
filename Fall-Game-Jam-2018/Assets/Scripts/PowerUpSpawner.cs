using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

    public GameObject powerup;
    public float minTimeBetweenSpan;
    public float maxTimeBetweenSpan;

    private float spawnCooldown;

	// Use this for initialization
	void Start () {
        spawnCooldown = getCooldownTime();
        powerup.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!powerup.activeSelf) {
            spawnCooldown -= Time.deltaTime;
            if (spawnCooldown <= 0.0f)
            {
                powerup.SetActive(true);
                spawnCooldown = getCooldownTime();
            }
        }
	}

    float getCooldownTime()
    {
        return Random.Range(minTimeBetweenSpan, maxTimeBetweenSpan + 1.0f);
    }
}
