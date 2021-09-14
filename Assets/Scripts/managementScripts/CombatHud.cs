using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CombatHud : MonoBehaviour
{
    public AbilityButton[] abilityButtons;
    public StatBar TimeBar;
    public StatBar HealthBar;
    public StatBar entropyBar;
    public PlayerTurn playerTurn;
    public Image characterIcon;
    public TMP_Text nameText;


    public TMP_Text StrText;
    public TMP_Text DexText;
    public TMP_Text IntText;
    public StatBar StamBar;
    public TMP_Text conText;
    public TMP_Text wisText;
    public TMP_Text chaText;
    public void Start()
    {
        PrimaryCharacter.primaryStatChange += updateStats;
        PrimaryCharacter.updateTImeBar += updateTimeBar;
        PrimaryCharacter.updateEntropyBar += updateEntropyBar;
        PrimaryCharacter.updateHealthBar += updateHealthBar;
    }
    public void OnDestroy() {

        PrimaryCharacter.primaryStatChange -= updateStats;
        PrimaryCharacter.updateTImeBar -= updateTimeBar;
        PrimaryCharacter.updateEntropyBar -= updateEntropyBar;
        PrimaryCharacter.updateHealthBar -= updateHealthBar;
    }
    public void updateTimeBar(Character myCharacter)
    {
        if (playerTurn.activeCharacter == myCharacter)
        {
            TimeBar.UpdateBar(myCharacter.time, myCharacter.maxTime);
        }
    }
    public void updateEntropyBar(Character myCharacter)
    {
        if (playerTurn.activeCharacter == myCharacter)
        {
            entropyBar.UpdateBar(myCharacter.entropy, 100);
        }
    }
    public void updateHealthBar(Character myCharacter)
    {
        if (playerTurn.activeCharacter == myCharacter)
        {
            HealthBar.UpdateBar(myCharacter.Hp, myCharacter.MaxHp);
        }
    }
    public void loadCharacter(PrimaryCharacter myCharacter) {

        changeCharacter(myCharacter);
    }
    public void updateStats(PrimaryCharacter myCharacter) {

        if (playerTurn.activeCharacter == myCharacter)
        {
            StrText.text = "Strength:"+myCharacter.Strength.ToString();
            DexText.text = "Dexterity:" + myCharacter.Dexterity.ToString();
            IntText.text = "Intelligence:" + myCharacter.Intelligence.ToString();
            StamBar.UpdateBar(myCharacter.Stamina, myCharacter.MaxStamina);
            conText.text = "Constitution:" + myCharacter.Constitution.ToString();
            wisText.text = "Wisdom:" + myCharacter.wisdom.ToString();
            chaText.text = "Charisma:" + myCharacter.charisma.ToString();
}
    }
    public void changeCharacter(PrimaryCharacter myCharacter) {
        int i;
        for (i = 0; i < myCharacter.myAbilities.Count; i++)
        {
            abilityButtons[i].gameObject.SetActive(true);
            abilityButtons[i].loadAbility(myCharacter.myAbilities[i]);
        }
        for (; i < abilityButtons.Length; i++)
        {
            abilityButtons[i].gameObject.SetActive(false);
        }
        HealthBar.UpdateBar(myCharacter.Hp, myCharacter.MaxHp);
        TimeBar.UpdateBar(myCharacter.time, myCharacter.maxTime);
        entropyBar.UpdateBar(myCharacter.entropy, 100);
        characterIcon.sprite = myCharacter.myCharacter.myImage;
        nameText.text = myCharacter.unitName;
        updateStats(myCharacter);
    }
    public void toggleGameobject(GameObject toggledgameObject) {

        toggledgameObject.SetActive(!toggledgameObject.activeSelf);
    }
}
