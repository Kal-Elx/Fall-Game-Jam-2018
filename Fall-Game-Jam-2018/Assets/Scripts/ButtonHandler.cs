using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour {

    private string arenaScene = "SimpleArenaScene";

    public void PlayGame()
    {
        SceneManager.LoadScene(arenaScene);
        SceneManager.LoadScene("PlayerScene", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void setScene(int n)
    {
        switch (n)
        {
            case 0:
                arenaScene = "SimpleArenaScene";
                break;
            case 1:
                arenaScene = "RotatingArenaScene";
                break;
            case 2:
                arenaScene = "SimpleArenaScene";
                break;
        }
    }
}
