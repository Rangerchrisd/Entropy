using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject openingObject;
    //does this for new game
    //buttonScript
    public void startGame() {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += loadStartGame;
        SceneManager.LoadScene(1);
    }
    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= loadStartGame;
    }
    //it does this AFTER the scene is loaded, for a new game
    public void loadStartGame(Scene scene, LoadSceneMode mode) {
        if (openingObject)
            Instantiate(openingObject, Vector3.zero, Quaternion.identity);
        SceneManager.sceneLoaded -= loadStartGame;

        Destroy(this.gameObject);
    }
    //buttonScript
    public void loadGame()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += loadContinueGame;
        SceneManager.LoadScene(1);
    }
    //it does this AFTER the scene is loaded
    public void loadContinueGame(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= loadContinueGame;
        Destroy(this.gameObject);

    }
    public void quitGame() {
        Application.Quit();
    }
    
}
