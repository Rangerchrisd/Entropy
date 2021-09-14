using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerTurn : MonoBehaviour
{
    #region decleration
    public List<Hex> path;
    public List<Hex> possibleTargets;
    public GameManager gameManager;
    public List<PrimaryCharacter> myParty;
    public float unitHeightOffset = .4f;

    public bool doorOpened;

    public bool showUI = false;
    [ConditionalHide("showUI", true)]
    public GameObject combatHUDGameObject,NonCombatHUD,BackToNonCombat;
    [ConditionalHide("showUI", true)]
    public CombatHud combatHUD;


    public bool showState = false;
    [ConditionalHide("showState", true)]
    public float currentTime = 0;
    [ConditionalHide("showState", true)]
    public int characterActiveIndex;
    [ConditionalHide("showState", true)]
    public PrimaryCharacter activeCharacter;
    [ConditionalHide("showState", true)]
    public bool acceptInstructions = true, hasLoaded = false;
    [ConditionalHide("showState", true)]
    public Ability targettingAbility;
    [ConditionalHide("showState", true)]
    public Interactable interactable;
    [ConditionalHide("showState", true)]
    public Hex target;

    public static event Action resetPathing;
    public static event Action onStartTurn;
    public static event Action onStartRound;
    public static event Action onEndTurn;
    #endregion
    #region getters

    public List<Hex> allHexes() {

        return gameManager.combatManager.allHexes;
    }
    public bool hasCharacter(string characterName)
    {
        foreach (Character i in myParty)
        {
            if (i.unitName == characterName)
            {
                return true;
            }
        }
        return false;
    }

    #endregion
    public void newRound()
    {
        foreach (Character i in myParty)
            i.readyCombat();
        changeCharacter(characterActiveIndex);
        onStartTurn?.Invoke();
        onStartRound?.Invoke();
    }
    #region subscribed
    //called when object is loaded in scene
    public void Start()
    {
        Hex.onClick += ChooseHex;
        Unit.onClick += ChooseHex;
        PrimaryCharacter.primaryDespawn += handlePrimaryCharacterDespawn;
        NeutralUnit.OnClick += ChooseHex;
        DoorTile.doorOpened += DoorTile_doorOpened;
    }

    private void DoorTile_doorOpened(DoorTile arg1, Hex arg2)
    {
        doorOpened = true;
    }

    public void handlePrimaryCharacterDespawn(PrimaryCharacter primaryCharacterDespawned) {
        {
            interruptMoving();
            if (activeCharacter == primaryCharacterDespawned)
            {
                changeCharacter(0);
            }
            else {
                resetDistanceMove();
            }
        }
    }
    public void OnDestroy()
    {
        Hex.onClick -= ChooseHex;
        Unit.onClick -= ChooseHex;
        PrimaryCharacter.primaryDespawn -= handlePrimaryCharacterDespawn;
        NeutralUnit.OnClick -= ChooseHex;
        DoorTile.doorOpened -= DoorTile_doorOpened;
    }
    #endregion
    public void endTurn()
    {
        onEndTurn?.Invoke();
        gameManager.combatManager.TurnEnded(turnType.PlayerTurn);

    }
    //called once per frame
    void Update()
    {
        //if characters are loaded and active unit is ready
        if (hasLoaded&& gameManager.combatManager.activeTurn == turnType.PlayerTurn&& !gameManager.isPaused)
        {

            if (doorOpened)
            {
                resetDistanceMove();
                doorOpened = false;
                return;
            }
            //waits for animation time
            if (currentTime <= 0&& acceptInstructions)
            {
                CheckInput();
                //if player has clicked left or right
                if (target)
                {
                    //process target and set to null
                    changeTarget();

                }
                else if (path.Count > 0)
                {
                    //if there is still pathing, move
                    nextStep();
                }
            }
            else
            {
                //continue animation time
                currentTime -= Time.deltaTime * gameManager.playerSettings.gameSpeed;
            }
        }
    }

    /*
    public void primaryCharacterLoadSheets()
    {
        int i = 0;
        foreach (PrimaryCharacter character in gameManager.combatManager.primaryCharacters)
        {
            character.loadCharacterSheet(gameManager.characterSheets[i]);
            i++;
        }
    }
    */

    #region targetting
    //process targetting
    public void changeTarget()
    {
        //if you have an ability loaded
        if (targettingAbility)
        {
            //if valid target, do ability then return to move
            if (possibleTargets.Contains(target))
            {
                targettingAbility.doAbility(target, activeCharacter);
                targettingAbility = null;
                resetDistanceMove();
            }
        }
        else
        {
            //if mouse is left clicking
            if (Input.GetMouseButtonDown(0))
            {
                //if space is empty
                if (!target.myUnit)
                {
                    //if not in combat or the player has enough time to do the action
                    if ((!gameManager.combatManager.inCombat || activeCharacter.time >= target.distanceTo) && target.distanceTo != 999)
                    {
                        //get the pathing towards the area
                        path = target.copyHexPath();
                        //remove own square
                        path.RemoveAt(0);
                        //cancel the previous interaction
                        interactable = null;
                    }
                }
            }
            //if right Clicking
            if (Input.GetMouseButtonDown(1))
            {
                //if the target has an interactable
                if (target.myInteractable)
                {
                    // if the interactable can be interacted with by adjacent squares
                    if (target.myInteractable.nextSpaceInteractable)
                    {
                        //if the space next to it is empty
                        if (!target.Path[target.Path.Count - 1].myUnit)
                        {
                            //if you are not in combat or you have enough energy AND the target is mapped
                            if ((!gameManager.combatManager.inCombat || activeCharacter.time >= target.distanceTo - 1) && target.distanceTo != 999)
                            {
                                //get the pathing towards the area
                                path = target.copyHexPath();
                                //remove own square
                                path.RemoveAt(0);
                                //should remove the final step unless you are in the square
                                if (path.Count > 0)
                                    path.RemoveAt(path.Count - 1);
                                //save interactable for when pathing is done
                                interactable = target.myInteractable;
                                //if you were in the space you were interacting with
                                if (path.Count == 0)
                                {
                                    //interact and clear interaction
                                    interactable.interact(activeCharacter);
                                    interactable = null;
                                }
                            }
                        }
                        else
                        {
                            //space is full
                        }
                    }
                    else
                    {

                        //if interactable needs for you to be in that square
                        // if the square is empty
                        if (!target.myUnit)
                        {
                            //if you are not in combat or you have enough energy AND the target is mapped
                            if ((!gameManager.combatManager.inCombat || activeCharacter.time >= target.distanceTo) && target.distanceTo != 999)
                            {
                                //copy pathing, remove own space, save interactable
                                path = target.copyHexPath();
                                path.RemoveAt(0);
                                interactable = target.myInteractable;
                                if (path.Count == 0)
                                {
                                    //interact and clear interaction
                                    interactable.interact(activeCharacter);
                                    interactable = null;
                                }
                            }
                        }
                    }
                }
                else
                {

                    //if the square has a unit
                    if (target.myUnit)
                    {
                        //if that unit has an interactable
                        if (target.myUnit.myInteract)
                        {
                            //if that unit can be interacted with outside of the space
                            if (target.myUnit.myInteract.nextSpaceInteractable)
                            {
                                //exception catch in case you click on your own unit which is interactable
                                // if the space next to it is empty
                                if (target.Path.Count <= 2 || 
                                    (!target.Path[target.Path.Count - 2].myUnit|| target.Path[target.Path.Count - 2].myUnit==activeCharacter))
                                {

                                    //Debug.Log(target.Path.Count <= 2);
                                    //Debug.Log(!target.Path[target.Path.Count - 2].myUnit);
                                    //Debug.Log(target.Path[target.Path.Count - 2].myUnit == activeCharacter);
                                    //if you are not in combat or you have enough energy AND the target is mapped
                                    if ((!gameManager.combatManager.inCombat || activeCharacter.time >= target.distanceTo - 1) && target.distanceTo != 999)
                                    {
                                        //copy pathing
                                        path = target.copyHexPath();
                                        //remove own space
                                        path.RemoveAt(0);
                                        //should remove the final step unless you are in the square
                                        if (path.Count > 0)
                                            path.RemoveAt(path.Count - 1);
                                        //save interactable
                                        interactable = target.myUnit.myInteract;
                                        if (path.Count == 0)
                                        {
                                            //interact and clear interaction
                                            interactable.interact(activeCharacter);
                                            interactable = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //
        target = null;
    }

    //sellect square with unit
    public void ChooseHex(Unit thisUnit)
    {
        //if unit was clicked and the unit has a hex
        if (thisUnit.myHex)
            ChooseHex(thisUnit.myHex);
    }
    //sellect square
    public void ChooseHex(Hex thisSquare)
    {
        // if not null
        if (!thisSquare)
            return;
        //if the game is not stunned
        if (acceptInstructions)
            target = thisSquare;
    }
    #endregion
    #region moveFunctions
    //move towards path, called when you have
    public void nextStep()
    {
        // if in combat use player time
        if (gameManager.combatManager.inCombat)
        {
            activeCharacter.time -= 1;
        }
        //give animation time
        currentTime = 3;
        //get next square
        Hex nextStepHex = path[0];
        //having gone there you don't need to go there anymore
        path.RemoveAt(0);
        //change which hex you are on
        activeCharacter.myHex.myUnit = null;
        activeCharacter.myHex = nextStepHex;
        //check for square effects
        nextStepHex.enterHex(activeCharacter);
        //set new hex
        nextStepHex.myUnit = activeCharacter;
        //Sets character model at correct height
        activeCharacter.transform.position = nextStepHex.transform.position + Vector3.up * unitHeightOffset;
        //this is to invoke after moving into the new square
        activeCharacter.moveIntoSquare();
        //Reset Pathing
        resetDistanceMove();
        //if you are finished moving, set move to 0
        if (path.Count == 0)
        {
            //set animation time to done
            currentTime = 0;
            //if you were interacting with a square, interact with it
            if (interactable)
            {
                interactable.interact(activeCharacter);
                //reset interactable
                interactable = null;
            }
        }
    }
    //if you got interuppted stop moving
    public void interruptMoving()
    {
        path.Clear();
        interactable = null;
        if (targettingAbility) {
            targettingAbility.outsideCancelTarget();
            targettingAbility = null;
        }
        //resetDistanceMove();
    }
    public void resetDistanceFight()
    {
        resetPathing?.Invoke();
        activeCharacter.myHex.findDistance();
        ChangeColor(gameManager.combatManager.findInRangeHexMove(activeCharacter.time), Color.blue);
    }
    public void resetDistanceMove()
    {
        resetPathing?.Invoke();
        activeCharacter.myHex.playerFindDistance();
        if (gameManager.combatManager.inCombat)
            ChangeColor(gameManager.combatManager.findInRangeHexMove(activeCharacter.time), Color.blue);
        if (!gameManager.combatManager.inCombat)
            ChangeColor(gameManager.combatManager.findInRangeHexMove(999), Color.blue);
        activeCharacter.myHex.changeSingleColor(Color.magenta);

    }
    #endregion
    #region CombatSetter
    #endregion
    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                changeCharacter(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                changeCharacter(1);
            }
        }
    }
    public void changeCharacter(int characterIndex)
    {
        if (characterIndex >= myParty.Count) {
            return;
        }
        interruptMoving();
        characterActiveIndex = characterIndex;
        //combatHUD.changeCharacter(myParty[characterIndex])
        activeCharacter = myParty[characterIndex];
        if (activeCharacter)
        {
            combatHUD.loadCharacter(activeCharacter);
            resetDistanceMove();
            path.Clear();
            combatHUD.updateHealthBar(activeCharacter);
            combatHUD.updateTimeBar(activeCharacter);
        }
    }

    public void ChangeColor(List<Hex> HexesToChange, Color toColor)
    {
        foreach (Hex i in gameManager.combatManager.allHexes)
        {
            if (i.myRender) {
                i.myRender.material.color = Color.black;
            }
        }
        foreach (Hex i in HexesToChange)
        {
            if (i.myRender)
            {
                i.myRender.material.color = toColor;
            }
        }
    }
}
