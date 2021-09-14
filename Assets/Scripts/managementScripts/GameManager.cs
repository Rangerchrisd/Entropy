using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region scriptPointers
    [Header("Script Pointers")]
    public bool showScriptPointers = false;
    [ConditionalHide("showScriptPointers", true)]
    public PlayerTurn playerTurn;
    [ConditionalHide("showScriptPointers", true)]
    public EnemyTurn enemyPlayerTurn;
    [ConditionalHide("showScriptPointers", true)]
    public CombatManager combatManager;
    [ConditionalHide("showScriptPointers", true)]
    public CameraController mainCamera;
    [ConditionalHide("showScriptPointers", true)]
    public Player player;
    [ConditionalHide("showScriptPointers", true)]
    public SaveGame saveGame;
    [ConditionalHide("showScriptPointers", true)]
    public PlayerSettings playerSettings;
    [ConditionalHide("showScriptPointers", true)]
    public TMP_Text toErr;
    [ConditionalHide("showScriptPointers", true)]
    public effectTextManager effectTextManager;
    #endregion
    //public List<CharacterSheet> characterSheets = new List<CharacterSheet>();
    public List<CharacterSheet> characterPool = new List<CharacterSheet>();
    public List<CharacterSheet> charactersToLoad = new List<CharacterSheet>();
    public List<Mission> missions = new List<Mission>();
    public Mission currentMission;
    public List<int> safeAreas = new List<int>();

    public bool isPaused;
    public GameObject playerSettingsPrefabs;

    public int VersionNumber;
    public float errorTime,timeToShowError;


    #region level loading stuff
    [Header("Script Pointers")]
    public bool showLevelLoading = false;
    [ConditionalHide("showLevelLoading", true)]
    public float currentTime, waitTime;
    [ConditionalHide("showLevelLoading", true)]
    public bool safeArea, loadingLevel;
    #endregion

    public void Start()
    {
        //Singleton
        if (FindObjectsOfType<GameManager>().Length!=1) {
            Destroy(this.gameObject);
        }

        playerSettings = FindObjectOfType<PlayerSettings>();
        // make sure they are all connected back
        playerTurn.gameManager = this;
        combatManager.gameManager = this;
        enemyPlayerTurn.gameManager = this;
        //make sure that this is not destroyed on load
        DontDestroyOnLoad(this.gameObject);

        //makes sure that when the scene is loaded that if it is a safe area that the 
        SceneManager.sceneLoaded += OnLoadScene;
    }
    public void OnDestroy()
    {
        //removes event
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    //makes sure that when the scene is loaded that if it is a safe area that the game is set to non combat
    public void OnLoadScene(Scene scene, LoadSceneMode mode) {
        //check if in safe areas
        if (safeAreas.Contains(scene.buildIndex)) {
            safeArea = true;
            playerTurn.NonCombatHUD.SetActive(false);
        }
        else {
            safeArea = false;
            playerTurn.NonCombatHUD.SetActive(true);
        }
    }

    //on game quit
    public void OnApplicationQuit()
    {

    }

    public void Update()
    {
        if (!playerSettings)
            playerSettings = Instantiate(playerSettingsPrefabs).GetComponent<PlayerSettings>();
        //gives enough time for first start and update
        if (loadingLevel)
        {
            if (currentTime > waitTime)
            {
                loadCharacters();
                loadingLevel = false;
                currentTime = 0;
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }
        if (timeToShowError == 0) { }
        else if (errorTime < timeToShowError)
        {
            errorTime += Time.deltaTime;
        }
        else {
            toErr.text = "";
            toErr.gameObject.SetActive(false);
        }
    }

    //loads main character and goes to first scene update 
    public void goBackHome()
    {
        charactersToLoad.Clear();
        charactersToLoad.Add(characterPool[0]);
        startLoad(1);
    }

    //this starts the loadup and saves changes from the previous area
    public void startLoad(int sceneIndex) {
        foreach (PrimaryCharacter i in combatManager.primaryCharacters)
            i.saveCharacterSheet();
        SceneManager.sceneLoaded += levelLoaded;
        SceneManager.LoadScene(sceneIndex);
    }

    //this resets all the on start important information, buffer loading characters, and stop calling this
    public void levelLoaded(Scene scene, LoadSceneMode mode)
    {
        playerTurn.allHexes().Clear();
        playerTurn.hasLoaded = false;
        combatManager.hasLoaded = false;
        loadingLevel = true;
        combatManager.newMap();
        SceneManager.sceneLoaded -= levelLoaded;
    }

    //Loading the characters based on the characters to load
    //assumes charactersToLoad isn't empty
    public void loadCharacters()
    {
        int i = 0;
        playerTurn.myParty.Clear();
        foreach (PrimaryCharacter character in combatManager.primaryCharacters)
        {
            if (charactersToLoad.Count > i)
            {
                character.loadCharacterSheet(charactersToLoad[i]);
            } else {
                Destroy(character.gameObject);
            }
            i++;
        }
        playerTurn.myParty = FindObjectsOfType<PrimaryCharacter>().ToList();
        playerTurn.changeCharacter(0);
    }
    public void printError(string errorStatement, float timetoshowerror) {
        toErr.text = errorStatement;
        errorTime = 0;
        timeToShowError = timetoshowerror;
        toErr.gameObject.SetActive(true);
    }
}
