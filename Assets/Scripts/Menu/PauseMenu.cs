using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject saveMenu;
    public GameObject loadMenu;
    public GameManager gameManager;
    public void quitGame() {
        Application.Quit();

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(!gameManager.isPaused)
                pauseGame();
            if (gameManager.isPaused)
                unPauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            toggleSettings();
        }
    }

    public void pauseGame() {
        toggleGameobject(pauseMenu);
        gameManager.isPaused = true;
        gameManager.playerSettings.settingsMenu.SetActive(false);
    }

    public void unPauseGame() {
        toggleGameobject(pauseMenu);
        gameManager.isPaused = false;
    }

    public void toggleSettings()
    {
        toggleGameobject(gameManager.playerSettings.settingsMenu);
        pauseMenu.SetActive(false);
        gameManager.isPaused = gameManager.playerSettings.settingsMenu.activeSelf;

    }

    public void toggleGameobject(GameObject toggledgameObject)
    {

        toggledgameObject.SetActive(!toggledgameObject.activeSelf);
    }

}
