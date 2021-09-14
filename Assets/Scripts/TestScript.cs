using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject dialoguePrefab;
    public GameObject missionSellect;
    public bool testFindType;
    public bool blockEvent;
    public bool savingGame;
    public bool loadingGame;

    public SaveGame saveGame;
    public LoadGame loadGame;
    public string saveFile;

    public void Start()
    {
        Unit.onTakeDamage += testBlock;
    }
    public void OnDestroy()
    {

        Unit.onTakeDamage -= testBlock;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            testDialogueScript();

        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            missionSellect.SetActive(true);
        }
        if (testFindType) {
            testFindUnit();
        }
        if (saveGame&&savingGame) {
            saveGame.saveGame(saveFile);
            savingGame = false;
        }
        if (loadGame && loadingGame)
        {
            loadGame.loadGame(saveFile);
            loadingGame = false;

        }
    }

    public void testDialogueScript() {

        if (dialoguePrefab) {
            Instantiate(dialoguePrefab);
        } else {
            Debug.Log("no dialogue prefab");
        
        }
    
    }
    public void testFindUnit() {
        Debug.Log(FindObjectsOfType<Unit>().Length);
    }
    public void testBlock(Unit takingDamage, float damage, Unit doingDamage) {
        if (blockEvent)
        {
            takingDamage.hp += damage;

            blockEvent = false;
        }
    }
}
