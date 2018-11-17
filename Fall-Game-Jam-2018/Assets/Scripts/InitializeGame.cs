using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeGame : MonoBehaviour {

	void Start () {
        SceneManager.LoadScene("MenuScene");
	}
}
