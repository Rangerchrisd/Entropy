using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newGame : MonoBehaviour
{
    public List<CharacterSheet> characterSheets;
    public GameManager gameManager;
    //time done to waitout race conditions
    public float waitTime;
    
    public float currentTime;
    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        newCharacterSheets();
        waitForLoad();
    }

    IEnumerator waitForLoad() {
        yield return new WaitForSeconds(10);
        loadCharacters();
    }
    public void Update()
    {
        if (currentTime > waitTime)
        {
            loadCharacters();
            Destroy(this);
        }
        else {
            currentTime += Time.deltaTime;
        }

    }

    public void newCharacterSheets()
    {
        gameManager.characterPool.Clear();
        //CharacterSheet me = newCharacterSheet();
        foreach (CharacterSheet character in characterSheets)
        {
            //gameManager.characterPool.Add(character);
            gameManager.characterPool.Add(character.CopyCharacterSheet());
            //gameManager.characterSheets.Add(gameManager.characterPool[(gameManager.characterPool.Count - 1)]);
        }
    }

    public void loadCharacters()
    {
        gameManager.charactersToLoad.Clear();
        gameManager.charactersToLoad.Add(gameManager.characterPool[0]);
        gameManager.loadCharacters();
    }
}